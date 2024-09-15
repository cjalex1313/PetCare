using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Exceptions.Pets
{
    public class PetNotFoundExpcetion : BaseException
    {
        public PetNotFoundExpcetion(Guid petId)
        {
            StatusCode = (int) HttpStatusCode.NotFound;
            ErrorMessage = $"Pet with id {petId} not found";
        }
    }
}
