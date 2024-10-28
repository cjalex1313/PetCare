using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.Common;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Auth;
using PetCare.Shared.Entities.Auth;
using PetCare.Shared.Exceptions;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PetCare.Shared.Config;

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
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authService.Login(loginRequest.Username, loginRequest.Password);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _authService.GenerateRefreshToken(loginRequest.Username);
            LoginResult response = new LoginResult()
            {
                AccessToken = tokenString,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            await _authService.RegisterUser(registerRequest);
            return Ok(new BaseResponse()
            {
                Succeeded = true
            });
        }
        
        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequest request)
        {
            var user = await _authService.FacebookLogin(request.AccessToken);
            if (!user.Succeeded)
            {
                throw new BaseException(user.Error);
            }
            var token = await _authService.GetAccessToken(user.User);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _authService.GenerateRefreshToken(user.User.UserName ?? throw new InvalidOperationException());
            return Ok(new LoginResult()
            {
                AccessToken = tokenString,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Confirmation")]
        public async Task<ActionResult<BaseResponse>> EmailConfirmation([FromBody] EmailValidationRequest request)
        {
            var decodedToken = Base64UrlEncoder.Decode(request.Token);
            await _authService.ConfirmEmail(request.UserId, decodedToken);
            return Ok(new BaseResponse());
        }

        [HttpGet("Profile")]
        [Authorize]
        public ActionResult<UserProfile> GetUserProfile()
        {
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var username = User.Identity.Name;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null || username == null) {
                throw new BaseException("Token does not contain username and email claims");
            }
            var response = new UserProfile()
            {
                Email = email,
                Username = username
            };
            return Ok(response);
        }

        [HttpGet("Users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<GetUsersResponse>> GetUsers()
        {
            var users = await _authService.GetUsers();
            var userDTOs = users.Select(u =>
            {
                return new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName ?? ""
                };
            }).ToList();
            return Ok(new GetUsersResponse(userDTOs));
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<LoginResult>> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            var username = _authService.GetUsernameFromExpiredToken(refreshRequest.AccessToken);
            string newAccessToken = await _authService.RefreshAccessToken(username, refreshRequest.RefreshToken);
            return Ok(new LoginResult()
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshRequest.RefreshToken
            });
        }
    }
}
