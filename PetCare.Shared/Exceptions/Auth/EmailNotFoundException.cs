using System.Net;

namespace PetCare.Shared.Exceptions.Auth;

public class EmailNotFoundException : BaseException
{
    public EmailNotFoundException(string email)
    {
        this.StatusCode = (int)HttpStatusCode.NotFound;
        this.ErrorMessage = $"The email {email} was not found";
    }
}