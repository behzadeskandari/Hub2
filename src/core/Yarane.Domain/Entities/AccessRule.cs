using System.Reflection.Emit;

namespace Yarane.Domain.Entities;

public class AccessRule
{
    public int AccessRuleId { get; set; }

    public Guid AccessTypeCode { get; set; }

    public string? AccessTypeName { get; set; }

    public int? AccessTypeId { get; set; } // access type id 

    public string PageUrl { get; set; } = null!;

    public bool CanAccess { get; set; } // true false
    public DateTime CreatedAt { get; set; }
    public Int16? Status { get; set; } // 1 active 2 not active
    public string? Description { get; set; }
    public string? ServiceType { get; set; }// Menu / Service

}
