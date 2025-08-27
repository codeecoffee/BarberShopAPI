namespace BarberApi.Models;
[Flags]
public enum WorkingDays
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4,
    Thursday = 8,
    Friday = 16,
    Saturday = 32,
    Sunday = 64,
    Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
    Weekend = Saturday | Sunday,
    All = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
}