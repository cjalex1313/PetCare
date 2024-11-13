namespace PetCare.Shared.Entities.Auth;

public class UserProfile
{
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
}