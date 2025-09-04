using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using BarberApi.Data;
using BarberApi.Models;
using BarberApi.Services;
using Microsoft.AspNetCore.Identity.Data;


namespace BarberApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly BarberShopDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    public AuthController(BarberShopDbContext context, IConfiguration configuration, IPasswordService passwordService, ITokenService tokenService)
    {
        _context = context;
        _configuration = configuration;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            //check if username exists
            if (await _context.Users.AnyAsync(u=>u.Username == request.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }
            //hash password
            string hashpass = _passwordService.HashPassword(request.Password);
            if (string.IsNullOrEmpty(hashpass))
            {
                return StatusCode(500, new { message = "Password hashing failed" });
            }
            //create user
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashpass,
                Role = request.Role
            };
            //save to db
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //return {} with data
            return Ok(new { message = "User created", userId = user.Id, username = user.Username, role = user.Role.ToString() });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new{ message = "Something went wrong", error = ex.Message, details = ex.StackTrace });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            //find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });
            //Use injected service for pass verification
            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password" });

            //generate token
            string tokn = _tokenService.GenerateJwtToken(user);
            return Ok(new
            {
                message = "Login successful",
                token = tokn,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role.ToString()
                }
            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Login failed", error = ex.Message });
        }
    }
    
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
}