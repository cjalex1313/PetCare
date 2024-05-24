using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Email
{
    public static class EmailModule
    {
        public static void AddEmailModule(this IServiceCollection services,
        IConfiguration builderConfiguration)
        {
            var emailConfig = builderConfiguration.GetSection("EmailConfig").Get<EmailConfig>();
            if (emailConfig == null)
            {
                throw new Exception("Error - incorrect email config - unable to map email config");
            }
            services.AddSingleton<EmailConfig>(emailConfig);
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
