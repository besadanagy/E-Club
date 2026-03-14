using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "NationalId", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b51b2d95-78cd-437d-997e-f53398da6405", new DateTime(2026, 3, 14, 10, 20, 54, 682, DateTimeKind.Utc).AddTicks(8166), null, "AQAAAAIAAYagAAAAEMh9UUmukvPSXuE1s8YUm4UIXlLrFI5WyyJBwv8B9xE+5GFMvzp1ZKyYWft+I1Ij1w==", "a44a461d-7bc2-4c46-9acb-aaecb9751d57" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9098bab8-aadd-459e-87fc-73b95e90c757", new DateTime(2026, 3, 13, 22, 34, 29, 182, DateTimeKind.Utc).AddTicks(8606), "AQAAAAIAAYagAAAAEAEpMC784fBqYKwTdzZ75Qnr1XacIvKN1/ZgF5oU6qDaEV6N1LmD90jRgUdtefJ5/g==", "e1a361eb-a838-4fdd-87e5-49ca63683cf5" });
        }
    }
}
