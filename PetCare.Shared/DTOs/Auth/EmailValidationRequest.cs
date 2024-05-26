namespace PetCare.Shared.DTOs.Auth;

public class EmailValidationRequest : BaseRequest
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}