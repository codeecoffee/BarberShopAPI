using BarberApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberApi.Data
{
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentServiceMapping>()
                .HasKey(asm=> new { asm.AppointmentId, asm.ServiceId });
        
            modelBuilder.Entity<AppointmentServiceMapping>()
                .HasOne(asm => asm.Appointment)
                .WithMany(a => a.Services)
                .HasForeignKey(asm => asm.AppointmentId);
        
            modelBuilder.Entity<AppointmentServiceMapping>()
                .HasOne(asm=> asm.Service)
                .WithMany(s=> s.AppointmentServiceMappings)
                .HasForeignKey(asm => asm.ServiceId);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("datetime");
                    }

                    if (property.ClrType == typeof(TimeSpan) || property.ClrType == typeof(TimeSpan?))
                    {
                        property.SetColumnType("time");
                    }
                }
            }
            modelBuilder.Entity<BusinessSettings>()
                .Property(e => e.WorkingDays)
                .HasConversion(
                    v => string.Join(',', v.Select(d => (int)d)),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => (DayOfWeek)int.Parse(s))
                        .ToArray()
                );

            base.OnModelCreating(modelBuilder);
        }
    }


    
}

