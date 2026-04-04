using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddServicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "84dafe89-5c3c-4a65-800b-5866858ed77c", new DateTime(2026, 3, 19, 14, 44, 39, 608, DateTimeKind.Utc).AddTicks(6790), "AQAAAAIAAYagAAAAEJwuREXKvnOLeyV1yvuiJ8J2jCUb3k1NKKep0nE6mCnJqhosyUhcOttZV3bY4P7Ogw==", "ef02a33a-3d4b-4be6-b957-762af0f9112d" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 19, 14, 44, 39, 600, DateTimeKind.Utc).AddTicks(9841));

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "Description", "DisplayOrder", "Endpoint", "Icon", "ImageUrl", "IsActive", "Name", "Type", "UpdatedById", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 14, 44, 39, 607, DateTimeKind.Utc).AddTicks(3053), "Reserve your favorite football field", 1, "/book-field", "sports_soccer", null, true, "Book a Field", 0, null, null },
                    { 2, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 14, 44, 39, 607, DateTimeKind.Utc).AddTicks(3058), "Participate in upcoming tournaments", 2, "/join-tournament", "emoji_events", null, true, "Join Tournament", 1, null, null },
                    { 3, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 14, 44, 39, 607, DateTimeKind.Utc).AddTicks(3060), "Get one-on-one coaching", 3, "/personal-coaching", "sports", null, true, "Personal Coaching", 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreatedById",
                table: "Services",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UpdatedById",
                table: "Services",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40a1f88a-2279-4e86-a19d-1bfcdfc0ff8e", new DateTime(2026, 3, 17, 15, 1, 22, 560, DateTimeKind.Utc).AddTicks(5624), "AQAAAAIAAYagAAAAENtZFz65LwL60e/uSmCi8IxFKWMUatUzWjCLhaabCk39TLz5qsLBRLZrKCZH3/xlYA==", "2a12094b-4294-4d34-b0d8-3aef608374e4" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 17, 15, 1, 22, 557, DateTimeKind.Utc).AddTicks(7585));
        }
    }
}
