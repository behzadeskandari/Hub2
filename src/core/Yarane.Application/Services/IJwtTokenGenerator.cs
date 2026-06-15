using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarane.Contract.Dtos.Account;
using Yarane.Domain.Entities;

namespace Yarane.Application.Services
{
    public interface IJwtTokenGenerator
    {
        public Task<string> GenerateToken(Users user);

        Task<string> GetToken(Users user);

        public JwtTokenClaims ReadToken(string token);

        public Task<string> GenerateRefreshToken();
    }
}
