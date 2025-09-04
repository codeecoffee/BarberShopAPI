using BarberApi.Models;

namespace BarberApi.Services;

public interface ITokenService
{
    string GenerateJwtToken(User user);
}