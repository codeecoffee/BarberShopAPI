namespace BarberApi.Models;

public class Customer : PersonEntity
{
    public Guid? PreferredBarberId { get; set; }
    
    public virtual User User { get; set; }
    public virtual Barber? Barber { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}