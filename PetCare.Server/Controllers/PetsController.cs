using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Pets;

namespace PetCare.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class PetsController : BaseController
{
    private readonly IPetService _petService;

    public PetsController(IPetService petService)
    {
        this._petService = petService;
    }

    [HttpGet]
    public ActionResult<BaseResponseWithData<IEnumerable<PetDTO>>> GetUserPets()
    {
        var userId = this.GetUserId();
        var pets = _petService.GetUserPets(userId);
        var response = new BaseResponseWithData<IEnumerable<PetDTO>>()
        {
            Data = pets
        };
        return Ok(response);
    }
}