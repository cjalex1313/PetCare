using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.DTOs.Auth;
public class UserProfile
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}
