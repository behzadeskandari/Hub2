using System.ComponentModel.DataAnnotations;

namespace Yarane.Contract.Dtos;

public class UpdateUserProfileRequest
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public long? Phone { get; set; }

}
