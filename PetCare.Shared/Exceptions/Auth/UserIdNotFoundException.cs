using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Exceptions.Auth
{
    public class UserIdNotFoundException : BaseException
    {
        public UserIdNotFoundException(Guid userId)
        {
            this.StatusCode = (int)HttpStatusCode.NotFound;
            this.ErrorMessage = $"No user with ID {userId} found";
        }
    }
}
