namespace PetCare.Shared.Exceptions.Auth
{
    public class UserCreationException : BaseException
    {
        public UserCreationException()
        {
            StatusCode = 409;
            ErrorMessage = "Error while trying to create the user";
        }
    }
}
