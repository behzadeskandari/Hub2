using System.ComponentModel.DataAnnotations;

namespace Yarane.Domain.Entities;

public class PasswordHistory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }  // Nullable UserId
    public string? HashedPassword { get; set; }  // Nullable Hashed Password
    public DateTime? CreatedAt { get; set; }  // Nullable CreatedAt

}
