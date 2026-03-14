using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "93cf88d2-2eec-4e61-9e34-b901351830a9", new DateTime(2026, 3, 14, 11, 41, 6, 228, DateTimeKind.Utc).AddTicks(5495), "AQAAAAIAAYagAAAAEPUXdr+OKLKMCo/gaiP2BHFcYEkwp5Fh9tOfS4n+xmmkTzMRXknJodeLfwOZH7X8ug==", "0ed3289a-e665-4fa1-a822-b917f1cfda28" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b51b2d95-78cd-437d-997e-f53398da6405", new DateTime(2026, 3, 14, 10, 20, 54, 682, DateTimeKind.Utc).AddTicks(8166), "AQAAAAIAAYagAAAAEMh9UUmukvPSXuE1s8YUm4UIXlLrFI5WyyJBwv8B9xE+5GFMvzp1ZKyYWft+I1Ij1w==", "a44a461d-7bc2-4c46-9acb-aaecb9751d57" });
        }
    }
}
