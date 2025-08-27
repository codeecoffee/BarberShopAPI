namespace BarberApi.Models;

public class NotificationLog : BaseEntity
{
    public Guid AppointmentId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    
    public virtual Appointment Appointment { get; set; }
    
}

