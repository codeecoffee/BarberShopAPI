namespace BarberApi.Models;

public class AuditLog : BaseEntity
{
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public string Action { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    
    public virtual User User { get; set; }
}