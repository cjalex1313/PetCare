using Microsoft.AspNetCore.Mvc;
using PetCare.Shared.Exceptions.Auth;
using System.Security.Claims;

namespace PetCare.Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                throw new UserIdIncorrectException(userIdString ?? "");
            }
            return userIdString;
        }
    }
}
