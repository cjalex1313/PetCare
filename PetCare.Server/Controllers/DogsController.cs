﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Pets.Add;
using PetCare.Shared.DTOs.Pets.Dogs;

namespace PetCare.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DogsController : BaseController
    {
        private readonly IDogService _dogService;

        public DogsController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpPost]
        public ActionResult<DogDto> AddDog([FromBody] AddPetRequest request)
        {
            var userId = GetUserId();
            var result = _dogService.AddDog(new DogDto { Name = request.Name, DateOfBirth = request.DateOfBirth, Sex = request.Sex }, userId);
            return Ok(result);
        }
    }
}
