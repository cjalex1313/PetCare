namespace PetCare.Shared.Exceptions.Auth
{
    public class EmailAlreadyExistsException : BaseException
    {
        public EmailAlreadyExistsException(string email)
        {
            this.StatusCode = 403;
            this.ErrorMessage = $"An user with the email: {email} already exists";
        }
    }
}
