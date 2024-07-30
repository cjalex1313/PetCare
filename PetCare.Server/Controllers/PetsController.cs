using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
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
    public ActionResult<GetUserPetsResponse> GetUserPets()
    {
        var userId = this.GetUserId().ToString();
        var pets = _petService.GetUserPets(userId);
        var response = new GetUserPetsResponse(pets);
        return Ok(response);
    }
}