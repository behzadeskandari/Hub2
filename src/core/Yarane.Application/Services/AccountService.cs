using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yarane.Contract.Dtos.Account;
using Yarane.Domain.Entities;

namespace Yarane.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IJwtTokenGenerator _JwtService;
        public readonly SignInManager<Users> _signInManager;
        public readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IAccountService> _logger;
        public AccountService(IJwtTokenGenerator JwtService, SignInManager<Users> signInManager, UserManager<Users> userManager,
               RoleManager<IdentityRole> roleManager, ILogger<IAccountService> logger)
        {
            _JwtService = JwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        public async Task<UserDto> CraeteApplicationUserDto(Users user)
        {

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = await _JwtService.GenerateToken(user),
            };
        }

        public async Task<Users> FindByNameAsync(string UserName)
        {
            var record = await _userManager.FindByNameAsync(UserName);
            return record;
        }

        public async Task<bool> CheckPasswordSignInAsync(Users user, string Password, bool lockout)
        {
            return await _userManager.CheckPasswordAsync(user, Password);
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<UserDto> CreateApplicationUserDto(Users user)
        {

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                Password = user.Password,
                DateCreated = user.CreateAt,
                EmailConfirmed = user.EmailConfirmed,

            };

        }

        public async Task<Result<Users>> CreateUserAsync(Users userToAdd, string password)
        {
            var result = await _userManager.CreateAsync(userToAdd, password);
            if (result.Succeeded)
            {
                return Result.Ok(userToAdd);
            }
            return Result.Fail(result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<Result<Users>> CreateUserAsync(Users userToAdd)
        {

            var result = await _userManager.CreateAsync(userToAdd);
            if (result.Succeeded)
            {
                return Result.Ok(userToAdd);
            }
            return Result.Fail(result.Errors.Select(e => e.Description).ToArray());

        }

        public async Task<SignInResult> SignInUserAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            else
            {
                return SignInResult.Success;
            }
            //return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public async Task SignInUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return;
            }
            //await _signInManager.PasswordSignInAsync(user, password, true, false);

        }

        public async Task<Users?> FindByEmailAsync(string payloadEmail)
        {
            var record = await _userManager.FindByNameAsync(payloadEmail);
            //Return the task of userDto
            return record;
        }


        public async Task<Users> AddRoleAsync(Users user, string role)
        {
            if (await _userManager.IsInRoleAsync(user, role))
            {
                return user;
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to add role {role} to user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Update User.Role if used in JWT or UserDto
            user.Role = role; // Ensure this is persisted if needed
            await _userManager.UpdateAsync(user); // Persist changes if User.Role is stored in DB
            return user;
        }

        public Task<Users> FindByIdAsync(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);
            return user;
        }


        public async Task<Users> SignOutUserAsync(Users user, string role)
        {
            var roles = _userManager.IsInRoleAsync(user, role);

            if (roles.Result == true)
            {
                return Task.FromResult(user).Result;
            }
            else
            {
                await _signInManager.SignOutAsync();
            }

            return Task.FromResult(user).Result;
        }

        public async Task<(bool Succeeded, string UserId, IEnumerable<string> Errors)> CreateUserAsync(string userName, string email, string password)
        {
            var user = new Users
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true // Set to false if email confirmation is required
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
                return (true, user.Id, Array.Empty<string>());
            }

            _logger.LogWarning("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            return (false, null, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool Succeeded, string UserId, IEnumerable<string> Errors)> CreateUserAsync(
            string userName, string email, string password, string firstName, string lastName)
        {
            var result = await CreateUserAsync(userName, email, password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(result.UserId);

                // Add claims for first name and last name
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, firstName));
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, lastName));

                return result;
            }

            return result;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<IdentityUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> UpdateUserAsync(Users user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string role)
        {
            // Create role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    return false;
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }


        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<(bool Succeeded, string UserId, IEnumerable<string> Errors)> SignInAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, null, new[] { "User not found" });
            }
            await _signInManager.SignInAsync(user, true);
            var errors = new List<string>();
            return (true, user.Id, errors);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User signed out");
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }



        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }



        public async Task<IList<Claim>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<Claim>();
            }

            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<bool> AddClaimAsync(string userId, Claim claim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddClaimAsync(user, claim);
            return result.Succeeded;
        }

        public async Task<bool> RemoveClaimAsync(string userId, Claim claim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.RemoveClaimAsync(user, claim);
            return result.Succeeded;
        }

        public Task<Users?> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return _userManager.GetUserAsync(claimsPrincipal);
        }

        public Task<string> GenerateOtp(int length = 6)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            byte[] randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[randomBytes[i] % validChars.Length];
            }

            return Task.FromResult(new string(chars));
        }
    }

}
