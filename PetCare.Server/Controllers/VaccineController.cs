using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Server.DTOs;
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
        public IActionResult GetPetVaccines([FromRoute] Guid petId)
        {
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, petId);
            var vaccines = _vaccinesService.GetVaccinesForPet(petId);
            var vaccineDTOs = _mapper.Map<IEnumerable<VaccineDTO>>(vaccines);
            return Ok(vaccineDTOs);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetVaccine([FromRoute] Guid id)
        {
            var vaccine = _vaccinesService.GetVaccine(id);
            var userId = GetUserId();
            _petService.VerifyUserCanAccessPet(userId, vaccine.PetId);
            var dto = _mapper.Map<VaccineDTO>(vaccine);
            return Ok(dto);
            
        }

        [HttpPost]
        public IActionResult AddVaccine([FromBody] VaccineDTO vaccineDTO)
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
