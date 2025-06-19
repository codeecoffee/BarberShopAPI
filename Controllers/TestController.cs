using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberApi.Data;

namespace BarberApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly BarberShopDbContext _context;
        
        public TestController(BarberShopDbContext context)
        {
            _context = context;
        }

        [HttpGet("db-connection")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    return BadRequest(new
                    {
                        Status = "Failed",
                        Message = "Database connection failed",
                        Timestamp = DateTime.UtcNow
                    });
                }
                    return Ok(new
                    {
                        Status = "Success",
                        Message = "Database connection successful",
                        Timestamp = DateTime.UtcNow
                    });

                
            }catch(Exception e)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = e.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
    
}