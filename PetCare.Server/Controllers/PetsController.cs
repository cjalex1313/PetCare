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
    public ActionResult<IEnumerable<PetDto>> GetUserPets()
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

    [HttpPut("{id:guid}")]
    public ActionResult<PetDto> UpdatePet(PetDto pet)
    {
        var userId = this.GetUserId();
        var dbPet = _petService.GetPet(pet.Id);
        if (dbPet.UserId != userId)
        {
            throw new PetOwnershipException();
        }
        _petService.UpdatePet(pet);
        return Ok(pet);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<PetDto> GetPet([FromRoute] Guid id)
    {
        var userId = GetUserId();
        Pet pet = _petService.GetPet(id);
        if (pet.UserId != userId)
        {
            throw new PetOwnershipException();
        }
        var dto = PetDto.GetDTO(pet);
        return Ok(dto);
    }
}