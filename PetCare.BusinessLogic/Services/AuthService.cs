using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetCare.Email;
using PetCare.Shared.Config;
using PetCare.Shared.Exceptions;
using PetCare.Shared.Exceptions.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.BusinessLogic.Services
{
    public interface IAuthService
    {
        Task RegisterUser(string username, string email, string password);
        Task RegisterAdmin(string adminPassword, string adminEmail);
        Task<JwtSecurityToken> Login(string username, string password);
        Task ConfirmEmail(Guid userId, string token);
    }
    internal class AuthService : IAuthService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService, AppSettings appSettings)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _appSettings = appSettings;
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

        public async Task RegisterAdmin(string adminPassword, string adminEmail)
        {
            var admin = await _userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                var identityAdmin = await AddUser("admin", adminEmail, adminPassword);
                await AddRoleToUser(identityAdmin, "Admin");
            }
            else
            {
                admin.Email = adminEmail;
                admin.EmailConfirmed = true;
                await _userManager.UpdateAsync(admin);
                var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
                var result = await _userManager.ResetPasswordAsync(admin, token, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Error while setting admin password");
                }

                var roles = await _userManager.GetRolesAsync(admin);
                if (!roles.Contains("Admin"))
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        public async Task RegisterUser(string username, string email, string password)
        {
            await CheckIfUserExists(username, email);
            var identityUser = await AddUser(username, email, password);
            await AddRoleToUser(identityUser, "User");
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
                //audience:  _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
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

        private async Task AddRoleToUser(IdentityUser identityUser, string role)
        {
            var roleResult = await _userManager.AddToRoleAsync(identityUser, role);
            if (!roleResult.Succeeded)
            {
                throw new UserCreationException();
            }
        }

        private async Task CheckIfUserExists(string username, string email)
        {
            var emailExists = await _userManager.FindByEmailAsync(email);
            if (emailExists != null)
            {
                throw new EmailAlreadyExistsException(email);
            }

            var usernameExists = await _userManager.FindByNameAsync(username);
            if (usernameExists != null)
            {
                throw new UsernameAlreadyExistsException(username);
            }
        }
    }
}
