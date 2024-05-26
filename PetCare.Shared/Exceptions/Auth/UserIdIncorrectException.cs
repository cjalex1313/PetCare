namespace PetCare.Shared.Exceptions.Auth
{
    public class UserIdIncorrectException : BaseException
    {
        public UserIdIncorrectException(string userId)
        {
            this.StatusCode = 400;
            this.ErrorMessage = $"User contains incorrect claim for userId: {userId}";
        }
    }
}
