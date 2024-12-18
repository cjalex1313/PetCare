﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCare.Shared.DTOs.HealthCheck;

namespace PetCare.Server.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class HealthChecksController : ControllerBase
    {
        [HttpGet("Ping")]
        public ActionResult<PingDto> Ping()
        {
            return Ok(new PingDto()
            {
                Date = DateTime.UtcNow,
            });
        }
    }
}
