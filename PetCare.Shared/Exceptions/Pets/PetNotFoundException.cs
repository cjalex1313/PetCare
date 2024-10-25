using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Exceptions.Pets
{
    public class PetNotFoundException : BaseException
    {
        public PetNotFoundException(Guid petId)
        {
            StatusCode = (int) HttpStatusCode.NotFound;
            ErrorMessage = $"Pet with id {petId} not found";
        }
    }
}
