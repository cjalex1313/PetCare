namespace PetCare.Shared.DTOs.Auth;

public class ResetPasswordRequest
{
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}