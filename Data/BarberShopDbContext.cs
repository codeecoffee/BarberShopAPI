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
            base.OnModelCreating(modelBuilder);
            
            ConfigureBaseEntity(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureCustomer(modelBuilder);
            ConfigureBarber(modelBuilder);
            ConfigureAppointment(modelBuilder);
            ConfigureAppointmentService(modelBuilder);
            ConfigureAppointmentServiceMapping(modelBuilder);
            ConfigureBusinessSettings(modelBuilder);
            ConfigureNotificationLog(modelBuilder);
            ConfigureAuditLog(modelBuilder);
        }
        
        private void ConfigureBaseEntity(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    //Ensure entity proper Id configuration
                    modelBuilder.Entity(entityType.ClrType).HasKey("Id");
                    
                    //Add Index for common queries
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex("CreatedAt")
                        .HasDatabaseName($"IX_{entityType.GetTableName()}_CreatedAt");
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex("IsActive")
                        .HasDatabaseName($"IX_{entityType.GetTableName()}_IsActive");
                }
            }
        }
        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<User>();
            builder.ToTable("Users");
            
            builder.Property(u=>u.Username)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u=>u.PasswordHash)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>(); //convert Enum into String 
            //Unique Constrains
            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");
            //Relationships
            builder.HasOne(u=>u.Customer)
                .WithOne(c=> c.User)
                .HasForeignKey<User>(c=>c.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(u=> u.Barber)
                .WithMany() //barber can have many users pointing to it
                .HasForeignKey(u=>u.BarberId)
                .OnDelete(DeleteBehavior.SetNull);
            //Business Rules
            builder.HasCheckConstraint("CK_User_RoleConsistency",
                "(Role = 'Customer' AND CustomerId IS NOT NULL AND BarberId IS NULL) OR " +
                "(Role = 'Barber' AND BarberId IS NOT NULL AND CustomerId IS NULL) OR " +
                "(Role = 'Admin' AND CustomerId IS NULL AND BarberId IS NULL)");
        }
        private void ConfigureCustomer(ModelBuilder modelBuilder)
        {
           var builder = modelBuilder.Entity<Customer>();
           
           builder.ToTable("Customers");
           
           builder.Property(c=>c.Name)
               .IsRequired()
               .HasMaxLength(100);
           builder.Property(c=>c.Email)
               .HasMaxLength(200);
           builder.Property(c=>c.Phone)
               .IsRequired()
               .HasMaxLength(20);
           builder.Property(c=>c.Picture)
               .HasMaxLength(500);
           
           //Unique Constrains
           builder.HasIndex(c=>c.Phone)
               .IsUnique()
               .HasDatabaseName("IX_Customer_Phone_Unique");
           builder.HasIndex(c => c.Email)
               .IsUnique()
               .HasDatabaseName("IX_Customer_Email_Unique")
               .HasFilter("Email IS NOT NULL");
           //Relationship
           builder.HasOne(c=>c.Barber)
               .WithMany(b=>b.PreferredByCustomers)
               .HasForeignKey(c=>c.PreferredBarberId)
               .OnDelete(DeleteBehavior.SetNull);
           builder.HasIndex(c => c.Name)
               .HasDatabaseName("IX_Customer_Name");
        }
        private void ConfigureBarber(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Barber>();
            builder.ToTable("Barbers");
            
            builder.Property(b=>b.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(b=>b.Email)
                .HasMaxLength(200);
            builder.Property(b=>b.Phone)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(b=>b.Picture)
                .HasMaxLength(500);
            builder.Property(b=>b.Specialities)
                .HasMaxLength(500);
            //Unique Constrains
            builder.HasIndex(b=>b.Phone)
                .IsUnique()
                .HasDatabaseName("IX_Barbers_Phone_Unique");
            builder.HasIndex(b=>b.Email)
                .IsUnique()
                .HasDatabaseName("IX_Barbers_Email_Unique")
                .HasFilter("Email IS NOT NULL");
            builder.HasIndex(b=>b.Name)
                .HasDatabaseName("IX_Barbers_Name");
        }
        private void ConfigureAppointment(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Appointment>();
            builder.ToTable("Appointments");
            
            builder.Property(a => a.AppointmentDateTime)
                .IsRequired();
            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>(); // Store enum as string
            builder.Property(a => a.Notes)
                .HasMaxLength(1000);
            
            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
            builder.HasOne(a => a.Barber)
                .WithMany(b => b.Appointments)
                .HasForeignKey(a => a.BarberId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasIndex(a => a.AppointmentDateTime)
                .HasDatabaseName("IX_Appointment_DateTime");
            builder.HasIndex(a => new { a.BarberId, a.AppointmentDateTime })
                .HasDatabaseName("IX_Appointment_Barber_DateTime");
            builder.HasIndex(a => new { a.CustomerId, a.AppointmentDateTime })
                .HasDatabaseName("IX_Appointment_Customer_DateTime");
            builder.HasIndex(a => a.Status)
                .HasDatabaseName("IX_Appointment_Status");
            
            // Business rules
            builder.HasCheckConstraint("CK_Appointment_NotInPast", 
                "AppointmentDateTime > NOW() OR Status IN ('Completed', 'Cancelled', 'NoShow')");
        }
        private void ConfigureAppointmentService(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<AppointmentService>();

            builder.ToTable("AppointmentServices");

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.DurationMinutes)
                .IsRequired();
            builder.Property(s => s.Price)
                .IsRequired()
                .HasPrecision(10, 2); // Decimal precision for money
            builder.Property(s => s.Description)
                .HasMaxLength(500);

            // Unique constraint
            builder.HasIndex(s => s.Name)
                .IsUnique()
                .HasDatabaseName("IX_AppointmentService_Name_Unique");

            // Business rules
            builder.HasCheckConstraint("CK_AppointmentService_PositiveDuration", 
                "DurationMinutes > 0");
            builder.HasCheckConstraint("CK_AppointmentService_PositivePrice", 
                "Price >= 0");
        }
        private void ConfigureAppointmentServiceMapping(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<AppointmentServiceMapping>();
            builder.ToTable("AppointmentServiceMappings");
            //composite primary key
            builder.HasKey(m => new { m.AppointmentId, m.ServiceId });
            //Relationships
            builder.HasOne(m => m.Appointment)
                .WithMany(a=> a.Services)
                .HasForeignKey(m=>m.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Service)
                .WithMany(a => a.AppointmentServiceMappings)
                .HasForeignKey(m=>m.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void ConfigureBusinessSettings(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<BusinessSettings>();
            builder.ToTable("BusinessSettings");

            builder.Property(s => s.OpeningTime)
                .IsRequired();
            builder.Property(s => s.ClosingTime)
                .IsRequired();
            builder.Property(s => s.WorkingDays)
                .IsRequired()
                .HasConversion<int>(); // Store flags enum as integer
            builder.Property(s => s.SlotDurationMinutes)
                .IsRequired();
            builder.Property(s => s.AdvanceBookingDays)
                .IsRequired();
            // Business rules
            builder.HasCheckConstraint("CK_BusinessSettings_ValidHours", 
                "OpeningTime < ClosingTime");
            builder.HasCheckConstraint("CK_BusinessSettings_PositiveSlotDuration", 
                "SlotDurationMinutes > 0");
            builder.HasCheckConstraint("CK_BusinessSettings_PositiveAdvanceBooking", 
                "AdvanceBookingDays > 0");
        }
        private void ConfigureNotificationLog(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<NotificationLog>();
            builder.ToTable("NotificationLogs");

            builder.Property(n => n.Type)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(n => n.Status)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(n => n.ErrorMessage)
                .HasMaxLength(1000);

            // Relationships
            builder.HasOne(n => n.Appointment)
                .WithMany(a => a.Notifications)
                .HasForeignKey(n => n.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Performance indexes
            builder.HasIndex(n => new { n.AppointmentId, n.Type })
                .HasDatabaseName("IX_NotificationLog_Appointment_Type");
            builder.HasIndex(n => n.Status)
                .HasDatabaseName("IX_NotificationLog_Status");
        }
        private void ConfigureAuditLog(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<AuditLog>();
            builder.ToTable("AuditLogs");

            builder.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.OldValues)
                .HasColumnType("json");
            builder.Property(a => a.NewValues)
                .HasColumnType("json");

            // Relationships
            builder.HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Performance indexes
            builder.HasIndex(a => new { a.EntityType, a.EntityId })
                .HasDatabaseName("IX_AuditLog_Entity");
            builder.HasIndex(a => a.CreatedAt)
                .HasDatabaseName("IX_AuditLog_CreatedAt");
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var businessSettingsId = Guid.NewGuid();
            modelBuilder.Entity<BusinessSettings>().HasData(new BusinessSettings
            {
                Id = businessSettingsId,
                OpeningTime = new TimeSpan(9, 0, 0),
                ClosingTime = new TimeSpan(18, 00, 0),
                WorkingDays = WorkingDays.Weekdays,
                SlotDurationMinutes = 30,
                AdvanceBookingDays = 30,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                IsActive = true
            });
            var haircutId = Guid.NewGuid();
            var beardTrimId = Guid.NewGuid();
            var washId = Guid.NewGuid();

            modelBuilder.Entity<AppointmentService>().HasData(
                new AppointmentService
                {
                    Id = haircutId,
                    Name = "Haircut",
                    DurationMinutes = 30,
                    Price = 25.00m,
                    Description = "Standard haircut and styling",
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new AppointmentService
                {
                    Id = beardTrimId,
                    Name = "Beard Trim",
                    DurationMinutes = 15,
                    Price = 15.00m,
                    Description = "Standard beard trimming",
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new AppointmentService
                {
                    Id = washId,
                    Name = "Hair wash",
                    DurationMinutes = 10,
                    Price = 10.00m,
                    Description = "Hair washing and conditioning",
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = currentTime;
                        entry.Entity.LastModifiedAt = currentTime;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = currentTime;
                        //Prevent overwriting CreateAt
                        entry.Property(e => e.CreatedAt).IsModified = false;
                        break;
                }
            }
        }
    }


    
}

