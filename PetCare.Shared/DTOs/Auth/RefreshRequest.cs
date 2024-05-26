namespace PetCare.Shared.DTOs.Auth;

public class RefreshRequest : BaseRequest
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
