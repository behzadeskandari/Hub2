using System.Security.Claims;
using AngleSharp.Dom;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NehzatMelli.Api.Controllers;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Messages.Authentication;
using ServiceAggregator.Api.Services;
using Yarane.Application.Services;
using Yarane.Common.Messages;
using Yarane.Contract.Dtos.Account;
using Yarane.Domain.Entities;

namespace Yarane.Api.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly IJwtTokenGenerator _jwt;
        private readonly IAccountService _accountService;
        private readonly SkiaSharpCaptchaService _skiaSharpCaptchaService;
        /// <summary>
        /// goes into the service not needed any more here 
        /// </summary>
        /// <param name="accountService"></param>
        public AccountController(IAccountService accountService, IJwtTokenGenerator jwtTokenGenerator
                    , SkiaSharpCaptchaService skiaSharpCaptchaService
            )
        {

            _jwt = jwtTokenGenerator;
            _accountService = accountService;
            _skiaSharpCaptchaService = skiaSharpCaptchaService;
        }

        [Authorize(Roles = "User,Staff")]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            var user = await _accountService.FindByNameAsync(User.FindFirst(ClaimTypes.Name)?.Value);
            var userDto = _accountService.CreateApplicationUserDto(user);
            userDto.Value.JWT = await _jwt.GetToken(user);
            return userDto;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginModel)
        {
            var isValid = _skiaSharpCaptchaService.ValidateCaptcha(loginModel.request!.Key, loginModel.request!.Captcha);
            if (!isValid)
                return Ok(new { IsSuccess = false, StatusCode = 2, Message = "کد امنیتی را صحیح وارد کنید" });

            Users user = await _accountService.FindByNameAsync(loginModel.UserName);
            if (user == null) return Unauthorized(ErrorMessages.InvalidUser);

            if (string.IsNullOrEmpty(user.Role) && user.Role == "Staff")
            {
                return Unauthorized(ErrorMessages.InvalidUser);
            }
            var results = await _accountService.CheckPasswordAsync(user.Id, loginModel.Password);

            if (!results) return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: ErrorMessages.InvalidPassword);//Unauthorized(ErrorMessages.InvalidPassword);

            var userDto = _accountService.CreateApplicationUserDto(user);
            var t = await _jwt.GetToken(user);
            userDto.Value.JWT = t;
            return Ok(new { message = SuccessMessages.LoginSuccess, Items = userDto });
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await _accountService.CheckEmailExistsAsync(registerDto.Email))
            {
                var user = await _accountService.GetUserByEmailAsync(registerDto.Email);
                var role = await _accountService.GetUserRolesAsync(user.Id);
                var Message = ErrorMessages.DuplicateEmail;

                if (role != null && user != null)
                {
                    if (registerDto.Email == user.Email && role.Contains("Staff"))
                    {
                        return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "You Aready Registerd as A Staff");
                    }
                }
                return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: Message);//Problem(statusCode: StatusCodes.Status400BadRequest, detail: Message);
            }
            var userToAdd = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Password = registerDto.Password,
                EmailConfirmed = true,
                AccessType = 1,
                AccessTypeCode = Guid.NewGuid(),
                UserId = Guid.NewGuid().ToString(),
                Address = registerDto.Address,
                EmailActiveCode = Guid.NewGuid().ToString(),
                CreateAt = DateTime.UtcNow,
                IsActivated = true,
                Role = "User",
                Id = Guid.NewGuid().ToString()
            };
            var result = await _accountService.CreateUserAsync(userToAdd, userToAdd.Password);
            if (result.IsSuccess)
            {
                // FIX: Fetch the fresh, tracked user record using the UserManager by their unique Email
                var freshUserRecord = await _accountService.FindByNameAsync(userToAdd.UserName);

                if (freshUserRecord == null)
                {
                    return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "User created but could not be retrieved.");
                }
                // Pass the officially tracked database record to your role manager service
                //var userRecord = await _accountService.AddRoleAsync(freshUserRecord, "Staff");

                return Ok(new { message = SuccessMessages.AccountCreated, Items = Enumerable.Empty<string>() });

                //var userRecord = await _accountService.AddRoleAsync(result.Value, "Staff");
                //return Ok(new { message = SuccessMessages.AccountCreated, Items = userRecord });
            }
            if (result.IsFailed)
            {
                return Ok(new { message = ErrorMessages.AccountCraetionFailed, Items = Enumerable.Empty<string>() });
            }
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: result.Errors.First().Message);//BadRequest(result.Errors);

        }



        [Authorize(Roles = "User,Staff")] // Requires a valid JWT token
        [HttpGet("check-login")]
        public async Task<IActionResult> CheckLogin()
        {
            // Get the user ID from the JWT token (stored in the User property)
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            Console.WriteLine($"User.Identity.IsAuthenticated: {User.Identity.IsAuthenticated}");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} => {claim.Value}");
            }

            if (email == null)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "No valid user found in token");//Unauthorized(new { IsLoggedIn = false, Message = "No valid user found in token" });
            }

            // Fetch the user from Identity
            var user = await _accountService.FindByEmailAsync(email);
            if (user == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: ErrorMessages.UserNotFound);//NotFound();
            }
            // Return success response with user details
            return Ok(new
            {
                IsLoggedIn = true,
                Email = user.UserName,
                Role = user.Role
            });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> LogOut(Users user, string role = "User")
        {
            var userRecord = _accountService.SignOutUserAsync(user, role);

            if (userRecord.Result is not null)
            {
                return Ok(new { message = SuccessMessages.LogOutSucess, Items = user });
            }

            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: ErrorMessages.ErrorInLogout);//BadRequest(new { message = ErrorMessages.ErrorInLogout });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!await _accountService.UserExistsByEmailAsync(request.Email))
            {
                // Don't reveal that the user does not exist
                return Ok();
            }

            var user = await _accountService.GetUserByEmailAsync(request.Email);
            var token = await _accountService.GeneratePasswordResetTokenAsync(user.Id);

            // In a real application, you would send an email with the token
            // For demo purposes, we'll just return it
            return Ok(new { token });
        }

        [HttpPost("getUserStatus")]
        public async Task<IActionResult> GetUserStatus([FromBody] JwtToken token)
        {
            //var tok = token as JwtToken;

            var responseToken = _jwt.ReadToken(token.token);

            if (responseToken != null && responseToken.UserId != null)
            {
                bool hasActiveCompany = false;
                var isSignedIn = true;
                return Ok(new { token, isSignedIn, hasActiveCompany });
            }
            else
            {
                var isSignedIn = false;
                var hasActiveCompany = false;
                return Ok(new { token, isSignedIn, hasActiveCompany });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Contract.Dtos.Account.ResetPasswordRequest request)
        {
            var user = await _accountService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok();
            }
            var result = await _accountService.ResetPasswordAsync(
                user.Id,
                request.Token,
                request.NewPassword);

            if (result)
            {
                return Ok();
            }

            return BadRequest(new { error = "Failed to reset password" });
        }

        [HttpGet("captcha")]
        [AllowAnonymous]
        public IActionResult GenerateCaptcha()
        {
            try
            {
                Console.WriteLine("Start GenerateCaptcha");
                var (image, key, text) = _skiaSharpCaptchaService.GenerateCaptchaImage();
                return Ok(new { image, key });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }

}
