using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetCare.BusinessLogic.Services;
using PetCare.Shared.Common;
using PetCare.Shared.DTOs;
using PetCare.Shared.DTOs.Auth;
using PetCare.Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PetCare.Shared.Entities.Auth;

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
            var token = await _authService.Login(loginRequest.Email, loginRequest.Password);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _authService.GenerateRefreshToken(loginRequest.Email);
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
            if (user is { Succeeded: false, Error: not null })
            {
                throw new BaseException(user.Error);
            }

            if (user.User == null)
            {
                throw new BaseException("Internal server error in facebook authentication");
            }
            var token = await _authService.GetAccessToken(user.User);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _authService.GenerateRefreshToken(user.User.Email ?? throw new InvalidOperationException());
            return Ok(new LoginResult()
            {
                AccessToken = tokenString,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.IdToken))
                return BadRequest("ID token is required");
            var token = await _authService.GoogleLogin(request.IdToken);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            LoginResult response = new LoginResult()
            {
                AccessToken = tokenString,
                RefreshToken = ""
            };
            return Ok(response);
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
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) {
                throw new BaseException("Token does not contain username and email claims");
            }
            var response = new UserProfileDTO()
            {
                Email = email,
            };
            UserProfile? userProfile = await _authService.GetUserProfile(User);
            if (userProfile != null)
            {
                response.FirstName = userProfile.FirstName;
                response.LastName = userProfile.LastName;
            }
            return Ok(response);
        }

        [HttpPut("SetUserNames")]
        [Authorize]
        public async Task<ActionResult> SetUserNames([FromBody] UserNamesDTO userNames)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            await _authService.SetUserNames(User, userNames);
            return Ok();
        }

        [HttpGet("Users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<GetUsersResponse>> GetUsers()
        {
            var users = await _authService.GetUsers();
            var userDTOs = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName ?? ""
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
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _authService.ResetPasswordAsync(request.UserId, request.Token, request.NewPassword);
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.SendForgotPasswordEmail(request.Email);
            return Ok();
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetUserId();
            await _authService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);
            return Ok();
        }
    }
}
