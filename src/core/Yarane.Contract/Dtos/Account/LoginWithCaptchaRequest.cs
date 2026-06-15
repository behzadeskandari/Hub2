using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Account
{
    public class LoginWithCaptchaRequest
    {
        /// <summary>
        /// عبارت امنیتی
        /// </summary>
        public string? Captcha { get; set; }
        /// <summary>
        /// کد رهگیری
        /// </summary>
        public string? Key { get; set; }
    }
}
