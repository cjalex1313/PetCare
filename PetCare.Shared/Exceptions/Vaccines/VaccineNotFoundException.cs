using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Exceptions.Vaccines
{
    public class VaccineNotFoundException : BaseException
    {
        public VaccineNotFoundException(Guid petId)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
            ErrorMessage = $"Vaccine with id {petId} not found";
        }
    }
}
