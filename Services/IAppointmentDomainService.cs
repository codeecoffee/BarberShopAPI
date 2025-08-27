namespace BarberApi.Services;

public interface IAppointmentDomainService
{
    Task<bool> CanScheduleAppointmentAsync(Guid baberId, DateTime dateTime, int durationMinutes );
    Task<decimal> CalculateAppointmentTotalAsync(List<Guid> serviceIds);
    Task<bool> IsWithinBusinessHoursAsync(DateTime appointmentTime);
}