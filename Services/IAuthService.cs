using BarberApi.Models;

namespace BarberApi.Services;

public interface IAuthService
{
    Task<string> LoginAsync(string username, string password);
    Task<User> RegisterCustomerAsync(string username, string password, string name, string email, string phone);
    Task<User> CreateBarberAccountAsync(string username, string password, string name, string email, string phone,
        string? specialities);

    string GenerateJwtToken(User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}