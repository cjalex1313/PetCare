using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Pets.Add;
using PetCare.Shared.DTOs.Pets.Cats;

namespace PetCare.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class CatsController : BaseController
{
    private readonly ICatsService _catsService;

    public CatsController(ICatsService catsService)
    {
        this._catsService = catsService;
    }

    [HttpPost]
    public ActionResult<CatDto> AddCat([FromBody] AddPetRequest request)
    {
        var userId = GetUserId();
        var result = _catsService.AddCat(new CatDto { Name = request.Name, DateOfBirth = request.DateOfBirth, Sex = request.Sex }, userId);
        return Ok(result);
    }
}
