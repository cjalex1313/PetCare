using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Config
{
    public class AppSettings
    {
        public required JwtConfig JWTConfig { get; set; }
        public required AdminConfig AdminConfig { get; set; }
        public required HangfireSettings HangfireSettings { get; set; }
        public required FacebookSettings FacebookSettings { get; set; }
        public required string EmailConfirmationUrl { get; set; }
    }
}
