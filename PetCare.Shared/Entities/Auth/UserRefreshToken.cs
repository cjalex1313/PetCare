using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Entities.Auth;
public class UserRefreshToken
{
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime ExpieryDate { get; set; }
}