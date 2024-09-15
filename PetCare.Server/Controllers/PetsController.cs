using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Pets;
using PetCare.Shared.Entities.Pets;
using PetCare.Shared.Exceptions.Pets;
using System.Diagnostics;

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
  public ActionResult<IEnumerable<PetDTO>> GetUserPets()
  {
    var userId = this.GetUserId();
    var pets = _petService.GetUserPets(userId);
    return Ok(pets);
  }

    [HttpDelete("{id:guid}")]
    public ActionResult<BaseResponse> DeletePet([FromRoute] Guid id)
    {
        string userId = this.GetUserId();
        _petService.DeletePet(id, userId);
        return Ok(new BaseResponse()
        {
            Succeeded = true
        });
    }

    [HttpGet("{id:guid}")]
    public ActionResult<BaseResponseWithData<Pet>> GetPet([FromRoute] Guid id) {
        var userId = GetUserId();
        Pet pet = _petService.GetPet(id);
        var response = new BaseResponseWithData<Pet>(pet);
        return Ok(response);
    }
} 