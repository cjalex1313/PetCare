using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Config
{
    public class AppSettings
    {
        public required JWTConfig JWTConfig { get; set; }
        public required AdminConfig AdminConfig { get; set; }
    }
}
