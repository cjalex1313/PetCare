using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.DTOs;
using PetCare.Shared.Entities;

namespace PetCare.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpcomingVaccineController : BaseController
    {
        private readonly IUpcomingVaccinesService _upcomingVaccinesService;
        private readonly IPetService _petService;
        private readonly IMapper _mapper;

        public UpcomingVaccineController(IUpcomingVaccinesService vaccinesService, IPetService petService, IMapper mapper)
        {
            _upcomingVaccinesService = vaccinesService;
            _petService = petService;
            _mapper = mapper;
        }

        [HttpGet("Pet/{petId:guid}")]
        public IActionResult GetPetVaccines([FromRoute] Guid petId)
        {
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, petId);
            var vaccines = _upcomingVaccinesService.GetUpcomingVaccinesForPet(petId);
            var vaccineDTOs = _mapper.Map<IEnumerable<UpcomingVaccineDTO>>(vaccines);
            return Ok(vaccineDTOs);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetVaccine([FromRoute] Guid id)
        {
            var vaccine = _upcomingVaccinesService.GetUpcomingVaccine(id);
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            var dto = _mapper.Map<UpcomingVaccineDTO>(vaccine);
            return Ok(dto);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteVaccine([FromRoute] Guid id)
        {
            var userId = GetUserId();
            var vaccine = _upcomingVaccinesService.GetUpcomingVaccine(id);
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            _upcomingVaccinesService.DeleteUpcomingVaccine(vaccine);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateVaccine([FromBody] UpcomingVaccineDTO vaccineDTO)
        {
            var vaccine = _upcomingVaccinesService.GetUpcomingVaccine(vaccineDTO.Id);
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            vaccine = _upcomingVaccinesService.UpdateUpcomingVaccine(vaccineDTO);
            var result = _mapper.Map<UpcomingVaccineDTO>(vaccine);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddVaccine([FromBody] UpcomingVaccineDTO vaccineDTO)
        {
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccineDTO.PetId);
            var vaccine = _mapper.Map<UpcomingVaccine>(vaccineDTO);
            _upcomingVaccinesService.AddUpcomingVaccine(vaccine);
            var result = _mapper.Map<UpcomingVaccineDTO>(vaccine);
            return Ok(result);
        }
    }
}
