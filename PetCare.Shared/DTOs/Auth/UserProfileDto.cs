namespace PetCare.Shared.DTOs.Auth;
public class UserProfileDto
{
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}