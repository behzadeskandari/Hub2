using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Account
{
    public class LoginResponse
    {
        /// <summary>
        /// توکن
        /// </summary>
        public string Token { get; set; } = "";
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string Username { get; set; } = "";
        /// <summary>
        /// تاریخ انقضای توکن
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }

}
