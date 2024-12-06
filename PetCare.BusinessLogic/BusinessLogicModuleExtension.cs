using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCare.BusinessLogic.Services;
using PetCare.DataAccess;
using PetCare.Email;
using PetCare.FileService;

namespace PetCare.BusinessLogic
{
    public static class BusinessLogicModuleExtension
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);
            services.AddEmailModule(configuration);
            services.AddFileSystemModule(configuration);
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<ICatsService, CatsService>();
            services.AddScoped<IDogService, DogService>();
            services.AddScoped<IVaccinesService, VaccinesService>();
            services.AddScoped<IUpcomingVaccinesService, UpcomingVaccinesService>();
        }
    }
}
