namespace PetCare.Shared.Exceptions.Auth
{
    public class PasswordIncorrectException : BaseException
    {
        public PasswordIncorrectException()
        {
            this.StatusCode = 401;
            this.ErrorMessage = "The password is incorrect";
        }
    }
}
