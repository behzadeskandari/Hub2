using System.ComponentModel.DataAnnotations;

namespace Yarane.Contract.Dtos
{
    public class RegisterUser
    {
        public RegisterUser()
        {

        }
        [Key]
        public Guid Id { get; set; }
        public int? AccessType { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public bool? isActivated { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public long Phone { get; set; }
        public short Status { get; set; }
        public string UserName { get; set; }
    }
}
