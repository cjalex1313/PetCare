using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetCare.DataAccess
{
  public static class DataAccessModuleExtension
  {
    public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<PetDbContext>(options =>
      {
        string? connectionString = configuration.GetConnectionString("PetCare");
        options.UseNpgsql(connectionString);
      });
    }
  }
}
