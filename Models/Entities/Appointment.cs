namespace BarberApi.Models;

public class Appointment : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid BarberId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public DateTime AppointmentEndDateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public virtual Customer Customer { get; set; }
    public virtual Barber Barber { get; set; }

    public virtual ICollection<AppointmentServiceMapping> Services { get; set; } =
        new List<AppointmentServiceMapping>();

    public virtual ICollection<NotificationLog> Notifications { get; set; } = new List<NotificationLog>();
}