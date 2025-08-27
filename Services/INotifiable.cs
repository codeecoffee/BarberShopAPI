namespace BarberApi.Services;

public interface INotifiable
{
    string Email { get; }
    string Phone { get; }
    string Name { get; }
}