using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using BarberApi.Data;
using BarberApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BarberApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class BarbersController : ControllerBase
{
    private readonly BarberShopDbContext _context;

    public BarbersController(BarberShopDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBarbers()
    {
        try
        {
            var barbers = await _context.Barbers
                .Where(b => b.IsActive)
                .Select(b => new BarberResponse
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Email = b.Email,
                        Phone = b.Phone,
                        Specialities = b.Specialities,
                        Picture = b.Picture,
                        CreatedAt = b.CreatedAt,
                    })
                .ToListAsync();
            return Ok(new { barbers = barbers, count = barbers.Count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error getting barbers", error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBarber(CreateBarberRequest request)
    {
        try
        {
            //check for barber phone number
            if(await _context.Barbers.AnyAsync(b=> b.Phone == request.Phone && b.IsActive))
                return BadRequest(new {message = "A barber with this phone number already exists."});
            //create obj barber
            var barber = new Barber
            {
                Name = request.Name.Trim(),
                Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
                Phone = request.Phone.Trim(),
                Specialities = string.IsNullOrWhiteSpace(request.Specialities) ? null : request.Specialities.Trim(),
                Picture = string.IsNullOrWhiteSpace(request.Picture) ? null : request.Picture.Trim()
            };
            //add to db
            _context.Barbers.Add(barber);
            // save changes
            await _context.SaveChangesAsync();
            // return 
            return CreatedAtAction(nameof(GetAllBarbers), new
            {
                message = "Barber created",
                barber = new BarberResponse
                {
                    Id = barber.Id,
                    Name = barber.Name,
                    Email = barber.Email,
                    Phone = barber.Phone,
                    Specialities = barber.Specialities,
                    Picture = barber.Picture,
                    CreatedAt = barber.CreatedAt
                }
            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating barber", error = ex.Message });
        }
    }
}

public class BarberResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public string? Specialities { get; set; }
    public string? Picture { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBarberRequest
{
    public string Name { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public string? Specialities { get; set; }
    public string? Picture { get; set; }
}