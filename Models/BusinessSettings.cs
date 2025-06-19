namespace BarberApi.Models;

public class BusinessSettings : BaseEntity
{
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public DayOfWeek[] WorkingDays { get; set; }
    public int SlotDurationMinutes { get; set; }
    public int AdvanceBookingDays { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
}