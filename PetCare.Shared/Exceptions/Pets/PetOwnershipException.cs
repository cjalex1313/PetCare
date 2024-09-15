using System.Net;

namespace PetCare.Shared.Exceptions.Pets
{
    public class PetOwnershipException : BaseException
    {
        public PetOwnershipException()
        {
            StatusCode = (int)HttpStatusCode.Forbidden;
            ErrorMessage = "You do not own this pet";
        }
    }
}
