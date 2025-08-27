using BarberApi.Services;

namespace BarberApi.Models;

public abstract class BaseEntity : IAuditable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        LastModifiedAt = DateTime.UtcNow;
    }
}