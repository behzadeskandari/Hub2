using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Account
{
    public class JwtToken
    {
        public string token { get; set; }
    }



    public class JwtTokenClaims
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
