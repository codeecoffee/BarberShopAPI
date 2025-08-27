namespace BarberApi.Models;

public class User: BaseEntity
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public Guid? BarberId { get; set; }
    public Guid? CustomerId { get; set; }
    
    public virtual Barber? Barber { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<Appointment> CreatedAppointments { get; set; } = new List<Appointment>();
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}

