using Microsoft.AspNetCore.Mvc;
using PetCare.Shared.Exceptions.Auth;
using System.Security.Claims;

namespace PetCare.Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid userId;
            var userIdOk = Guid.TryParse(userIdString, out userId);
            if (!userIdOk)
            {
                throw new UserIdIncorrectException(userIdString ?? "");
            }
            return userId;
        }
    }
}
