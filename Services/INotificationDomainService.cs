using BarberApi.Models;

namespace BarberApi.Services;

public interface INotificationDomainService
{
    Task<bool> ShouldSendReminderAsync(Appointment appointment);
    Task<NotificationType> GetNextNotificationTypeAsync();
}