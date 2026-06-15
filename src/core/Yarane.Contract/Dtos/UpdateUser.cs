using System.ComponentModel.DataAnnotations;

namespace Yarane.Contract.Dtos;

public class UpdateUser
{
    public UpdateUser()
    {

    }
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Password_confirm { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Address { get; set; }

    public long Phone { get; set; }

    public int Status { get; set; }

    public long Otp { get; set; }
    public bool IsActivate { get; set; }
    public int? AccessType { get; set; }

}
