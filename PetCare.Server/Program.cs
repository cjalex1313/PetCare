using Microsoft.AspNetCore.Identity;
using PetCare.DataAccess;
using PetCare.Shared.Config;
using PetCare.BusinessLogic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PetCare.Server.Middleware;
using PetCare.BusinessLogic.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using PetCare.Server.Mappers;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using PetCare.BusinessLogic.BackgroundJobs;
using PetCare.Shared.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
if (appSettings == null)
{
    throw new BaseException("Config is corrupt");
}
builder.Services.AddCors();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AppSettings>(appSettings);
builder.Services.AddBusinessLogic(builder.Configuration);
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<PetDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = appSettings.FacebookSettings.AppId;
    facebookOptions.AppSecret =  appSettings.FacebookSettings.AppSecret;

    // You can optionally request additional user information here
    facebookOptions.Fields.Add("name");
    facebookOptions.Fields.Add("email");
    facebookOptions.Fields.Add("picture"); 
}).AddJwtBearer(options =>
{
    options.SaveToken = false;
    options.RequireHttpsMetadata = false;
    var jwtSecret = appSettings.JWTConfig.Secret;
    if (string.IsNullOrWhiteSpace(jwtSecret))
    {
        throw new BaseException("Invalid JWT config");
    }
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

//AutoMapper
builder.Services.AddAutoMapper(typeof(VaccineProfile));

builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new List<string>()
                    }
                });
});
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

var options = new DashboardOptions
{
    Authorization = new[] { new HangfireCustomBasicAuthenticationFilter { User = appSettings.HangfireSettings.Username, Pass = appSettings.HangfireSettings.Password } }
};
app.UseHangfireDashboard("/hangfire", options);

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var petDbContext = scope.ServiceProvider.GetService<PetDbContext>();
    if (petDbContext == null)
    {
        throw new BaseException("Cannot initialize dbContext");
    }
    await petDbContext.Database.MigrateAsync();
    var adminRole = await petDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
    if (adminRole == null)
    {
        petDbContext.Roles.Add(new IdentityRole()
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "1"
        });
    }
    var userRole = await petDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
    if (userRole == null)
    {
        petDbContext.Roles.Add(new IdentityRole()
        {
            Name = "User",
            NormalizedName = "USER",
            ConcurrencyStamp = "2"
        });
    }
    await petDbContext.SaveChangesAsync();
}

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<IAuthService>()
        ?.EnsureAdminExists().Wait();
}

JobsScheduler.RegisterRecurringJobs();

app.Run();
