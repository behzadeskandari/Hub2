using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yarane.Contract.Dtos.Account;
using Yarane.Domain.Entities;

namespace Yarane.Application.Services
{
    public interface IAccountService
    {

        Task<UserDto> CraeteApplicationUserDto(Users user);
        Task<bool> CheckEmailExistsAsync(string email);

        Task<Users> FindByNameAsync(string UserName);

        Task<bool> CheckPasswordSignInAsync(Users user, string Password, bool lockout);
        Microsoft.AspNetCore.Mvc.ActionResult<UserDto> CreateApplicationUserDto(Users user);
        Task<Result<Users>> CreateUserAsync(Users userToAdd, string password);

        Task<Microsoft.AspNetCore.Identity.SignInResult> SignInUserAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task<Users?> FindByEmailAsync(string payloadEmail);

        Task<Result<Users>> CreateUserAsync(Users userToAdd);
        Task SignInUserAsync(string userName, string password);

        Task<Users> AddRoleAsync(Users user, string role);
        Task<Users> FindByIdAsync(string userId);

        Task<Users> SignOutUserAsync(Users user, string role);

        Task<(bool Succeeded, string UserId, IEnumerable<string> Errors)> CreateUserAsync(string userName, string email, string password);
        Task<(bool Succeeded, string UserId, IEnumerable<string> Errors)> CreateUserAsync(string userName, string email, string password, string firstName, string lastName);
        Task<bool> UserExistsAsync(string userId);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<IdentityUser> GetUserByIdAsync(string userId);
        Task<IdentityUser> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsUserInRoleAsync(string userId, string role);
        Task<bool> AddUserToRoleAsync(string userId, string role);
        Task<bool> RemoveUserFromRoleAsync(string userId, string role);
        Task<IList<string>> GetUserRolesAsync(string userId);

        // Authentication
        Task<bool> CheckPasswordAsync(string userId, string password);
        Task<(bool Succeeded, string UserId, IEnumerable<string> Errors)> SignInAsync(string email);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<bool> ConfirmEmailAsync(string userId, string token);

        // Password management
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        // Claims
        Task<IList<Claim>> GetUserClaimsAsync(string userId);
        Task<bool> AddClaimAsync(string userId, Claim claim);
        Task<bool> RemoveClaimAsync(string userId, Claim claim);

        Task<Users?> GetUserAsync(ClaimsPrincipal claimsPrincipal);

        Task<string> GenerateOtp(int length = 8);
    }

}
