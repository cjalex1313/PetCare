namespace PetCare.Shared.DTOs.Auth;

public class UpdateUserProfileDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
}