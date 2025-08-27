using BarberApi.Models;

namespace BarberApi.Services;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int hours = 24);
    Task<bool> IsBarberAvailableAsync(Guid barberId, DateTime dateTime, int durationMinutes );
}