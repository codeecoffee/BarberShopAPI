using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BarberApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppointmentServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentServices", x => x.Id);
                    table.CheckConstraint("CK_AppointmentService_PositiveDuration", "DurationMinutes > 0");
                    table.CheckConstraint("CK_AppointmentService_PositivePrice", "Price >= 0");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Specialities = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Picture = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusinessSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    SlotDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    AdvanceBookingDays = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessSettings", x => x.Id);
                    table.CheckConstraint("CK_BusinessSettings_PositiveAdvanceBooking", "AdvanceBookingDays > 0");
                    table.CheckConstraint("CK_BusinessSettings_PositiveSlotDuration", "SlotDurationMinutes > 0");
                    table.CheckConstraint("CK_BusinessSettings_ValidHours", "OpeningTime < ClosingTime");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    PreferredBarberId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Picture = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Barbers_PreferredBarberId",
                        column: x => x.PreferredBarberId,
                        principalTable: "Barbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Username = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false),
                    BarberId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    BarberId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AppointmentEndDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(255)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    EntityId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EntityType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OldValues = table.Column<string>(type: "json", nullable: true),
                    NewValues = table.Column<string>(type: "json", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppointmentServiceMappings",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ServiceId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentServiceMappings", x => new { x.AppointmentId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_AppointmentServiceMappings_AppointmentServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "AppointmentServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentServiceMappings_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<string>(type: "varchar(255)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationLogs_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AppointmentServices",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "DurationMinutes", "IsActive", "LastModifiedAt", "Name", "Price", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("16309cb8-6949-4a8a-8367-49e9b12213c9"), new DateTime(2025, 9, 3, 19, 4, 1, 358, DateTimeKind.Utc).AddTicks(313), null, "Hair washing and conditioning", 10, true, new DateTime(2025, 9, 3, 19, 4, 1, 358, DateTimeKind.Utc).AddTicks(314), "Hair wash", 10.00m, null },
                    { new Guid("55ac64dd-b3a4-4be4-ba7f-99ef0fdaed5d"), new DateTime(2025, 9, 3, 19, 4, 1, 358, DateTimeKind.Utc).AddTicks(310), null, "Standard beard trimming", 15, true, new DateTime(2025, 9, 3, 19, 4, 1, 358, DateTimeKind.Utc).AddTicks(310), "Beard Trim", 15.00m, null },
                    { new Guid("66da4319-f41d-43e6-a33c-7cb4e54936e5"), new DateTime(2025, 9, 3, 19, 4, 1, 358, DateTimeKind.Utc).AddTicks(298), null, "Standard haircut and styling", 30, true, new DateTime(2025, 9, 3, 19, 4, 1, 358, DateTimeKind.Utc).AddTicks(299), "Haircut", 25.00m, null }
                });

            migrationBuilder.InsertData(
                table: "BusinessSettings",
                columns: new[] { "Id", "AdvanceBookingDays", "ClosingTime", "CreatedAt", "CreatedBy", "IsActive", "LastModifiedAt", "OpeningTime", "SlotDurationMinutes", "UpdatedAt", "UpdatedBy", "WorkingDays" },
                values: new object[] { new Guid("dd4c2522-66b6-4c95-9138-1e45ec5b1458"), 30, new TimeSpan(0, 18, 0, 0, 0), new DateTime(2025, 9, 3, 19, 4, 1, 357, DateTimeKind.Utc).AddTicks(3355), null, true, new DateTime(2025, 9, 3, 19, 4, 1, 357, DateTimeKind.Utc).AddTicks(3356), new TimeSpan(0, 9, 0, 0, 0), 30, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 31 });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Barber_DateTime",
                table: "Appointments",
                columns: new[] { "BarberId", "AppointmentDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Barber_TimeSlot",
                table: "Appointments",
                columns: new[] { "BarberId", "AppointmentDateTime", "AppointmentEndDateTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Customer_DateTime",
                table: "Appointments",
                columns: new[] { "CustomerId", "AppointmentDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DateTime",
                table: "Appointments",
                column: "AppointmentDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CreatedAt",
                table: "Appointments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_IsActive",
                table: "Appointments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServiceMappings_ServiceId",
                table: "AppointmentServiceMappings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentService_Name_Unique",
                table: "AppointmentServices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_CreatedAt",
                table: "AppointmentServices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_IsActive",
                table: "AppointmentServices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Entity",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedBy",
                table: "AuditLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_IsActive",
                table: "AuditLogs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_CreatedAt",
                table: "Barbers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_Email_Unique",
                table: "Barbers",
                column: "Email",
                unique: true,
                filter: "Email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_IsActive",
                table: "Barbers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_Name",
                table: "Barbers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_Phone_Unique",
                table: "Barbers",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSettings_CreatedAt",
                table: "BusinessSettings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSettings_IsActive",
                table: "BusinessSettings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email_Unique",
                table: "Customers",
                column: "Email",
                unique: true,
                filter: "Email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Name",
                table: "Customers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Phone_Unique",
                table: "Customers",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedAt",
                table: "Customers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsActive",
                table: "Customers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PreferredBarberId",
                table: "Customers",
                column: "PreferredBarberId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLog_Appointment_Type",
                table: "NotificationLogs",
                columns: new[] { "AppointmentId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLog_Status",
                table: "NotificationLogs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_CreatedAt",
                table: "NotificationLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_IsActive",
                table: "NotificationLogs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BarberId",
                table: "Users",
                column: "BarberId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedAt",
                table: "Users",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                table: "Users",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentServiceMappings");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BusinessSettings");

            migrationBuilder.DropTable(
                name: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "AppointmentServices");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Barbers");
        }
    }
}
