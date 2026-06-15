using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "نام باید بین {0} حداقل {1} و حداکثر باید باشد  ")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "نام خانوادگی  باید بین {0} حداقل {1} و حداکثر باید باشد  ")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "رمز عبور  باید بین {0} حداقل {1} و حداکثر باید باشد  ")]
        public string Password { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
    }
}
