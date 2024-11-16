using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.DTOs.Auth;
public class RegisterRequest : BaseRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
