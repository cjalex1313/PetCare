using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Shared.DTOs.Pets.Cats;

namespace PetCare.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CatsController : BaseController
{
    [HttpPost]
    public ActionResult<AddCatResponse> AddCat([FromBody] AddCatRequest request)
    {
        var userId = GetUserId();

    }
}
