using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Yarane.Application.Services;
using Yarane.Common.Utilities;
using Yarane.Contract.Dtos.Account;
using Yarane.Domain.Entities;

namespace Yarane.Contract.Interfaces.UserService
{
    public class JwtService : IJwtTokenGenerator
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;
        public JwtService(IConfiguration config, IDistributedCache memoryCache)
        {
            _config = config;
            _cache = memoryCache;
            // jwtToken is used for bath encrypting and description the JWT token 
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));

        }
        public async Task<string> GenerateToken(Users user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role,user.Role),
            };

            var creadentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.Now.AddDays(int.Parse(_config["JWT:ExpiresInDays"])),
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
                SigningCredentials = creadentials,
            };
            //await _signInManager.SignInAsync(user, isPersistent: false);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(jwt);
            if (string.IsNullOrEmpty(token) || token.Split('.').Length != 3)
            {

                _cache.SetString(user.UserId,token , new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(int.Parse(_config["JWT:ExpiresInDays"])),
                });
                throw new Exception("Generated JWT is invalid");
            }
            return token;
        }

        public async Task<string> GetToken(Users user)
        {
            //string token = _cache.Get<string>(user.UserName);
            //if (token == null)
            //{
            var token = await GenerateToken(user);
            // _cache.Set(user.UserName, token);
            //}
            return token;
        }

        public JwtTokenClaims ReadToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token))
                throw new ArgumentException("Invalid JWT token");


            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);
            if (jwtToken.ValidTo <= DateTime.Now)
            {
                return new JwtTokenClaims();
            }
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var roles = jwtToken.Claims.FirstOrDefault(X => X.Value == "Staff" | X.Value == "Admin" | X.Value == "User");
            var given_name = jwtToken.Claims.FirstOrDefault(c => c.Type == "given_name");
            var family_name = jwtToken.Claims.FirstOrDefault(x => x.Type == "family_name");
            var userId = jwtToken.Claims.First().Value;

            return new JwtTokenClaims
            {
                UserId = userId,
                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty,
                FirstName = given_name.ToString(),
                LastName = family_name.ToString(),
                Role = roles.Value,
                ExpiresAt = jwtToken.ValidTo
            };
        }

        public Task<string> GenerateRefreshToken()
        {
            // Get refresh token length from configuration (default to 32 bytes if not specified)
            int tokenLength = int.TryParse(_config["JWT:RefreshTokenLength"], out var length) ? length : 32;

            // Generate a cryptographically secure random string
            byte[] randomBytes = RandomNumberGenerator.GetBytes(tokenLength);
            string refreshToken = Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "")
                .Substring(0, tokenLength);

            // Set expiration for refresh token (default to 7 days if not specified)
            int refreshTokenExpiryDays = int.TryParse(_config["JWT:RefreshTokenExpiresInDays"], out var expiry) ? expiry : 7;

            // Store the refresh token in cache with expiration
            _cache.SetString($"RefreshToken_{refreshToken}", refreshToken);
            return Task.FromResult(refreshToken);
        }
    }

}
