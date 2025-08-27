namespace BarberApi.Models;

public class BusinessSettings : BaseEntity
{
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public WorkingDays WorkingDays { get; set; } = WorkingDays.Weekdays;
    public int SlotDurationMinutes { get; set; }
    public int AdvanceBookingDays { get; set; }
    public DateTime UpdatedAt { get; set; }
}

