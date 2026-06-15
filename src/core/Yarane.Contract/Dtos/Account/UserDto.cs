using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Contract.Dtos.Account
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JWT { get; set; }
        public string Email { get; set; }
        public string? UserName { get; set; }
        public string PictureUrl { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; }
        public string Role { get; set; }

    }
}
