namespace PetCare.Shared.Exceptions.Auth
{
    public class UsernameAlreadyExistsException : BaseException
    {
        public UsernameAlreadyExistsException(string username)
        {
            this.StatusCode = 403;
            this.ErrorMessage = $"An user with the username: {username} already exists";
        }
    }
}
