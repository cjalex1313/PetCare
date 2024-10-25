using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.DTOs;
using PetCare.Shared.Entities;
using PetCare.Shared.Exceptions.Pets;

namespace PetCare.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VaccineController : BaseController
    {
        private readonly IVaccinesService _vaccinesService;
        private readonly IPetService _petService;
        private readonly IMapper _mapper;

        public VaccineController(IVaccinesService vaccinesService, IPetService petService, IMapper mapper)
        {
            _vaccinesService = vaccinesService;
            _petService = petService;
            _mapper = mapper;
        }

        [HttpGet("Pet/{petId:guid}")]
        public ActionResult<IEnumerable<VaccineDTO>> GetPetVaccines([FromRoute] Guid petId)
        {
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, petId);
            var vaccines = _vaccinesService.GetVaccinesForPet(petId);
            var vaccineDtos = _mapper.Map<IEnumerable<VaccineDTO>>(vaccines);
            return Ok(vaccineDtos);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<VaccineDTO> GetVaccine([FromRoute] Guid id)
        {
            var vaccine = _vaccinesService.GetVaccine(id);
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            var dto = _mapper.Map<VaccineDTO>(vaccine);
            return Ok(dto);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteVaccine([FromRoute] Guid id)
        {
            var userId = GetUserId();
            var vaccine = _vaccinesService.GetVaccine(id);
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            _vaccinesService.DeleteVaccine(vaccine);
            return Ok();
        }

        [HttpPut]
        public ActionResult<VaccineDTO> UpdateVaccine([FromBody] VaccineDTO vaccineDTO)
        {
            var vaccine = _vaccinesService.GetVaccine(vaccineDTO.Id);
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            vaccine = _vaccinesService.UpdateVaccine(vaccineDTO);
            var result = _mapper.Map<VaccineDTO>(vaccine);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<VaccineDTO> AddVaccine([FromBody] VaccineDTO vaccineDTO)
        {
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccineDTO.PetId);
            var vaccine = _mapper.Map<Vaccine>(vaccineDTO);
            _vaccinesService.AddVaccine(vaccine);
            var result = _mapper.Map<VaccineDTO>(vaccine);
            return Ok(result);
        }
    }
}
