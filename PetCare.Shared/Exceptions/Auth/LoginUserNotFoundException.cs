namespace PetCare.Shared.Exceptions.Auth
{
    public class LoginUserNotFoundException : BaseException
    {
        public LoginUserNotFoundException(string username)
        {
            this.StatusCode = 404;
            this.ErrorMessage = $"The username {username} was not found";
        }
    }

}
