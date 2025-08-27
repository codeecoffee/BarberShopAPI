namespace BarberApi.Models;

public class AppointmentService : BaseEntity
{
    public string Name { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    
    public virtual ICollection<AppointmentServiceMapping> AppointmentServiceMappings { get; set; } = new List<AppointmentServiceMapping>();
    
}