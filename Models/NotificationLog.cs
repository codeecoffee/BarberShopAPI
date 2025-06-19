namespace BarberApi.Models;

public class NotificationLog : BaseEntity
{
    public Guid AppointmentId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    
    public virtual Appointment Appointment { get; set; }
    
}

public enum NotificationType {Confirmation, Reminder24H, Reminder1H, Cancellation}
public enum NotificationStatus {Pending, Cancelled, Sent, Failed}