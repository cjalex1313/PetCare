using Microsoft.AspNetCore.Identity;

namespace PetCare.BusinessLogic.Facebook;

public class FacebookAuthResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }
    public IdentityUser? User { get; set; }
}