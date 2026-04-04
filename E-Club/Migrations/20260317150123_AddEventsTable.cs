using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    CurrentParticipants = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40a1f88a-2279-4e86-a19d-1bfcdfc0ff8e", new DateTime(2026, 3, 17, 15, 1, 22, 560, DateTimeKind.Utc).AddTicks(5624), "AQAAAAIAAYagAAAAENtZFz65LwL60e/uSmCi8IxFKWMUatUzWjCLhaabCk39TLz5qsLBRLZrKCZH3/xlYA==", "2a12094b-4294-4d34-b0d8-3aef608374e4" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "CurrentParticipants", "Description", "EndDate", "ImageUrl", "IsFeatured", "Location", "MaxParticipants", "StartDate", "Status", "Title", "UpdatedById", "UpdatedOn" },
                values: new object[] { 1, null, new DateTime(2026, 3, 17, 15, 1, 22, 557, DateTimeKind.Utc).AddTicks(7585), 45, "Join us for the biggest football event of the year!", new DateTime(2023, 10, 26, 18, 0, 0, 0, DateTimeKind.Utc), null, true, "Main Arena, Smart Club", 100, new DateTime(2023, 10, 24, 9, 0, 0, 0, DateTimeKind.Utc), 0, "Annual Football Championship", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedById",
                table: "Events",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UpdatedById",
                table: "Events",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "93cf88d2-2eec-4e61-9e34-b901351830a9", new DateTime(2026, 3, 14, 11, 41, 6, 228, DateTimeKind.Utc).AddTicks(5495), "AQAAAAIAAYagAAAAEPUXdr+OKLKMCo/gaiP2BHFcYEkwp5Fh9tOfS4n+xmmkTzMRXknJodeLfwOZH7X8ug==", "0ed3289a-e665-4fa1-a822-b917f1cfda28" });
        }
    }
}
