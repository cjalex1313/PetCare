using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.Common;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Auth;
using PetCare.Shared.Entities.Auth;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace PetCare.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authService.Login(loginRequest.Username, loginRequest.Password);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _authService.GenerateRefreshToken(loginRequest.Username);
            var response = new LoginResponse()
            {
                AccessToken = tokenString,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }

        [HttpPost("Confirmation")]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailValidationRequest request)
        {
            var decodedToken = Base64UrlEncoder.Decode(request.Token);
            await _authService.ConfirmEmail(request.UserId, decodedToken);
            return Ok(new BaseResponse());
        }

        [HttpGet("Users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetUsers();
            var userDTOs = users.Select(u =>
            {
                return new UserDTO
                {
                    Id = u.Id,
                    Username = u.UserName ?? ""
                };
            }).ToList();
            return Ok(new GetUsersResponse(userDTOs));
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            var username = _authService.GetUsernameFromExpiredToken(refreshRequest.AccessToken);
            string newAccessToken = await _authService.RefreshAccessToken(username, refreshRequest.RefreshToken);
            return Ok(new LoginResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshRequest.RefreshToken
            });
        }
    }
}
