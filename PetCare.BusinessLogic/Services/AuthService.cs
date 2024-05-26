using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetCare.DataAccess;
using PetCare.Email;
using PetCare.Shared.Common;
using PetCare.Shared.Config;
using PetCare.Shared.Entities.Auth;
using PetCare.Shared.Exceptions;
using PetCare.Shared.Exceptions.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PetCare.BusinessLogic.Services
{
    public interface IAuthService
    {
        Task EnsureAdminExists();
        Task EnsureRolesExistInDb();
        Task<JwtSecurityToken> Login(string username, string password);
        Task<string> GenerateRefreshToken(string username);
        string GetUsernameFromExpiredToken(string token);
        Task<string> RefreshAccessToken(string username, string refreshToken);
        Task<IList<IdentityUser>> GetUsers();
        Task ConfirmEmail(Guid userId, string token);

    }
    internal class AuthService : IAuthService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;
        private readonly PetDbContext _dbContext;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService, AppSettings appSettings, RoleManager<IdentityRole> roleManager, PetDbContext petDbContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _appSettings = appSettings;
            _roleManager = roleManager;
            _dbContext = petDbContext;
        }

        public async Task EnsureAdminExists()
        {
            var adminEmail = _appSettings.AdminConfig.Email;
            var adminPassowrd = _appSettings.AdminConfig.Password;
            var admin = await _userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                var identityAdmin = await AddUser("admin", adminEmail, adminPassowrd);
                await AddRoleToUser(identityAdmin, Roles.Admin);
            }
            else
            {
                admin.Email = adminEmail;
                admin.EmailConfirmed = true;
                await _userManager.UpdateAsync(admin);
                var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
                var result = await _userManager.ResetPasswordAsync(admin, token, adminPassowrd);
                if (!result.Succeeded)
                {
                    throw new Exception("Error while setting admin password");
                }

                var roles = await _userManager.GetRolesAsync(admin);
                if (!roles.Contains(Roles.Admin))
                {
                    await _userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }

        public async Task<IList<IdentityUser>> GetUsers()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.User);
            return users;
        }

        public async Task EnsureRolesExistInDb()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(Roles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var jwtSecret = _appSettings.JWTConfig.Secret;
            if (jwtSecret == null)
            {
                throw new BaseException("JWT configuration invalid");
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var token = new JwtSecurityToken(
                issuer: _appSettings.JWTConfig.ValidIssuer,
                audience: _appSettings.JWTConfig.ValidAudience,
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        public async Task<JwtSecurityToken> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new LoginUserNotFoundException(username);
            }
            var loginAttempt = await _userManager.CheckPasswordAsync(user, password);
            if (!loginAttempt)
            {
                throw new PasswordIncorrectException();
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GetToken(authClaims);
            return token;
        }

        private async Task AddRoleToUser(IdentityUser identityUser, string role)
        {
            var roleResult = await _userManager.AddToRoleAsync(identityUser, role);
            if (!roleResult.Succeeded)
            {
                throw new UserCreationException();
            }
        }

        public string GetUsernameFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWTConfig.Secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new BaseException("Invalid access token");
            if (principal == null || principal.Identity == null || principal.Identity.Name == null)
            {
                throw new BaseException("Invalid access token");
            }
            return principal.Identity.Name;
        }

        private async Task<IdentityUser> AddUser(string username, string email, string password)
        {
            var identityUser = new IdentityUser()
            {
                UserName = username,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
            {
                throw new UserCreationException();
            }
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            if (confirmationToken != null)
            {
                var encodedToken = Base64UrlEncoder.Encode(confirmationToken);
                _emailService.SendEmail(new Email.Models.MailData
                {
                    Email = email,
                    Name = username,
                    Subject = "Email confirmation",
                    Body = $"Welcome to PetCare. Click <a href=\"{_appSettings.EmailConfirmationUrl + "?userId=" + identityUser.Id + "&token=" + encodedToken}\">here</a> to confirm your email"
                }, MimeKit.Text.TextFormat.Html);
            }
            return identityUser;
        }

        public async Task<string> GenerateRefreshToken(string username)
        {
            var randomNumber = new byte[32];
            string refreshToken = "";
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new BaseException("User not found for refresh token generation");
            }

            var dbRefreshToken = _dbContext.UserRefreshTokens.FirstOrDefault(urt => urt.UserId == user.Id);
            if (dbRefreshToken != null)
            {
                dbRefreshToken.RefreshToken = refreshToken;
                dbRefreshToken.ExpieryDate = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                dbRefreshToken = new UserRefreshToken
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    ExpieryDate = DateTime.UtcNow.AddYears(1)
                };
                _dbContext.UserRefreshTokens.Add(dbRefreshToken);
            }
            _dbContext.SaveChanges();

            return refreshToken;
        }

        public async Task<string> RefreshAccessToken(string username, string refreshToken)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new BaseException()
                {
                    ErrorMessage = $"User with username {username} not found",
                    StatusCode = 404
                };
            }
            var dbRefreshToken = _dbContext.UserRefreshTokens.FirstOrDefault(urt => urt.UserId == user.Id);
            if (dbRefreshToken == null || dbRefreshToken.RefreshToken != refreshToken)
            {
                throw new BaseException()
                {
                    ErrorMessage = "Error while trying to refresh access token",
                    StatusCode = 400
                };
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GetToken(authClaims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public async Task ConfirmEmail(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UserIdNotFoundException(userId);
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result == null || !result.Succeeded)
            {
                throw new BaseException()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = result != null ? String.Join(". ", result.Errors.Select(e => e.Description).ToList()) : "Error while confirming email via confirmation token"
                };
            }
        }
    }
}
