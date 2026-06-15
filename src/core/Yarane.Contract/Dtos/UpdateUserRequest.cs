using System.ComponentModel.DataAnnotations;

namespace Yarane.Contract.Dtos;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public long Phone { get; set; }
    public int Status { get; set; }
    public bool IsActivate { get; set; }
    public int? AccessType { get; set; }
    public string? UserId { get; set; }

}
