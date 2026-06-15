using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Yarane.Domain.Entities
{
    public class Users : IdentityUser
    {
        public Users()
        {

        }
        #region properties
        public string Id { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Role { get; set; }
        public string? Address { get; set; }
        public string? EmailActiveCode { get; set; }
        public bool?  IsActivated { get; set; }
        public long? Phone { get; set; }
        public long? Otp { get; set; }
        public string? Token { get; set; }
        public int? Status { get; set; }
        public int? AccessType { get; set; }
        public Guid? AccessTypeCode { get; set; }
        public string? UserId { get; set; }
        public bool? IsLogin { get; set; }
        public int? FailedLoginAttempts { get; set; }
        public bool? IsLocked { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? LockoutEndTime { get; set; } 

        #endregion

    }
}
