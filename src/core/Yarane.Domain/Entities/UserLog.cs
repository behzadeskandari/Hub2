using Microsoft.VisualBasic;

namespace Yarane.Domain.Entities;

public class UserLog
{
    public Int64 Id { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime? CreateAt { get; set;}
    public string? Ip { get; set;}
    public Int16? Status { get; set; }
}
