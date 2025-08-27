namespace BarberApi.Models;

public enum NotificationType
{
    Confirmation, 
    Reminder24H, 
    Reminder1H, 
    Cancellation
}
public enum NotificationStatus
{
    Pending, 
    Cancelled, 
    Sent, 
    Failed
}
