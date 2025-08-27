using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BarberApi.Data;
using BarberApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BarberApi.Models.Entities;

public class AuthService: IAuthService
{
    private readonly BarberShopDbContext _context;
    private readonly IConfiguration _configuration;
    
    public AuthService(BarberShopDbContext context, IConfiguration configuration)
        => (_context, _configuration) = (context, configuration);

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _context.Users.Include(u=> u.Barber)
            .Include(u=> u.Customer)
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if(user == null || !VerifyPassword(password, user.PasswordHash)) 
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User is not active");
        }

        return GenerateJwtToken(user);
    }

    public async Task<User> RegisterCustomerAsync(string username, string password, string name, string email, string phone)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        if (await _context.Customers.AnyAsync(c => c.Email == email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var customer = new Customer { Name = name, Email = email, Phone = phone };
        var user = new User { Username = username, PasswordHash = HashPassword(password), Role = UserRole.Customer, Customer = customer };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<User> CreateBarberAccountAsync(string username, string password, string name, string email, string phone,
        string? specialities)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            throw new InvalidOperationException("Username already exists");
        }
        if (await _context.Barbers.AnyAsync(b => b.Email == email))
        {
            throw new InvalidOperationException("Email already exists");
        }
        var barber = new Barber {Name = name, Email = email, Phone = phone, Specialities = specialities};
        var user = new User {Username = username, PasswordHash = HashPassword(password), Role = UserRole.Barber, Barber = barber};
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    //Token and password operations 
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        if (user.BarberId.HasValue)
        {
            claims.Add(new Claim("BarberId", user.BarberId.Value.ToString()));
        }

        if (user.CustomerId.HasValue)
        {
            claims.Add(new Claim("CustomerId", user.CustomerId.Value.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature), Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
        
    }
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
