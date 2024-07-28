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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
if (appSettings == null)
{
    throw new Exception("Config is corrupt");
}
builder.Services.AddCors();
builder.Services.AddSingleton<AppSettings>(appSettings);
builder.Services.AddBusinessLogic(builder.Configuration);
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<PetDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = false;
    options.RequireHttpsMetadata = false;
    var jwtSecret = appSettings.JWTConfig.Secret;
    if (string.IsNullOrWhiteSpace(jwtSecret))
    {
        throw new Exception("Invalid JWT config");
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

builder.Services.AddControllers();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var petDbContext = scope.ServiceProvider.GetService<PetDbContext>();
    if (petDbContext == null)
    {
        throw new Exception("Cannot initialize dbContext");
    }
    petDbContext.Database.Migrate();
    var adminRole = petDbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
    if (adminRole == null)
    {
        petDbContext.Roles.Add(new IdentityRole()
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "1"
        });
    }
    var userRole = petDbContext.Roles.FirstOrDefault(r => r.Name == "User");
    if (userRole == null)
    {
        petDbContext.Roles.Add(new IdentityRole()
        {
            Name = "User",
            NormalizedName = "USER",
            ConcurrencyStamp = "2"
        });
    }
    petDbContext.SaveChanges();
}

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<IAuthService>()
        ?.EnsureAdminExists().Wait();
}

app.Run();
