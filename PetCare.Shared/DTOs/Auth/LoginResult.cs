using PetCare.Shared.DTOs;

namespace PetCare.Shared.DTOs.Auth;

public class LoginResult
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
