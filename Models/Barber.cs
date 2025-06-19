namespace BarberApi.Models;

public class Barber: PersonEntity
{
    public string? Specialities { get; set; }
    
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<Customer> PreferredByCustomers { get; set; } = new List<Customer>();
}