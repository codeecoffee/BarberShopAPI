namespace BarberApi.Models;

public class AppointmentServiceMapping
{
    public Guid AppointmentId { get; set; }
    public Guid ServiceId { get; set; }
    
    public virtual Appointment Appointment { get; set; }
    public virtual AppointmentService Service { get; set; }
    
}