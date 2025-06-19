using BarberApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberApi.Data;

public class BarberShopDbContext : DbContext
{
    public BarberShopDbContext(DbContextOptions<BarberShopDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Barber> Barbers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentService> AppointmentServices { get; set; }
    public DbSet<AppointmentServiceMapping> AppointmentServiceMappings { get; set; }
    public DbSet<BusinessSettings> BusinessSettings { get; set; }
    public DbSet<NotificationLog> NotificationLogs { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {}
}