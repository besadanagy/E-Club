using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddBannersAndSports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ActionUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_Banners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banners_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banners_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegisteredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sports_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sports_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SportClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SportId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    CurrentParticipants = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SportClasses_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SportClasses_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SportClasses_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportClassId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassBookings_SportClasses_SportClassId",
                        column: x => x.SportClassId,
                        principalTable: "SportClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9aeeebaa-847d-4803-82dc-6fcb6aac9b79", new DateTime(2026, 4, 22, 1, 1, 57, 101, DateTimeKind.Utc).AddTicks(7549), "AQAAAAIAAYagAAAAEBoIoOKR9WTJV5ApNrJPk97+Z4oNnY+e52FAQ8L2vW2B+uiSh3sDwif73kNUdN847g==", "c1e14f18-8fc6-4af6-b4b0-ca9934ba436b" });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "ActionUrl", "CreatedById", "CreatedOn", "DisplayOrder", "ImageUrl", "IsActive", "Subtitle", "Title", "Type", "UpdatedById", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "/join-tournament", "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, "/images/banners/tournament.jpg", true, "Registrations open for the Summer Cup.", "New Tournament", 0, null, null },
                    { 2, "/personal-coaching", "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, "/images/banners/coaching.jpg", true, "Train with professional coaches.", "Personal Coaching", 2, null, null },
                    { 3, "/register", "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "/images/banners/membership.jpg", true, "Join our elite sports community today.", "Club Membership", 2, null, null }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 22, 1, 1, 57, 92, DateTimeKind.Utc).AddTicks(8974));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 22, 1, 1, 57, 95, DateTimeKind.Utc).AddTicks(9470));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 22, 1, 1, 57, 95, DateTimeKind.Utc).AddTicks(9473));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 22, 1, 1, 57, 95, DateTimeKind.Utc).AddTicks(9475));

            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "DisplayOrder", "Icon", "ImageUrl", "IsActive", "Name", "UpdatedById", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1, "sports_soccer", null, true, "Football", null, null },
                    { 2, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 2, "sports_basketball", null, true, "Basketball", null, null },
                    { 3, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "sports_tennis", null, true, "Tennis", null, null }
                });

            migrationBuilder.InsertData(
                table: "SportClasses",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "CurrentParticipants", "Description", "EndTime", "ImageUrl", "Location", "MaxParticipants", "Price", "SportId", "StartTime", "Status", "Title", "Type", "UpdatedById", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, new DateTime(2026, 4, 25, 12, 0, 0, 0, DateTimeKind.Utc), null, "Pitch A", 20, 50m, 1, new DateTime(2026, 4, 25, 10, 0, 0, 0, DateTimeKind.Utc), 0, "Pro Football Drill", 0, null, null },
                    { 2, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, new DateTime(2026, 4, 25, 16, 0, 0, 0, DateTimeKind.Utc), null, "Room 201", 15, 30m, 1, new DateTime(2026, 4, 25, 14, 0, 0, 0, DateTimeKind.Utc), 0, "Tactics & Strategy", 0, null, null },
                    { 3, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 15, null, new DateTime(2026, 4, 28, 18, 0, 0, 0, DateTimeKind.Utc), null, "Main Stadium", 50, 100m, 1, new DateTime(2026, 4, 28, 9, 0, 0, 0, DateTimeKind.Utc), 0, "Weekend Football Cup", 1, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_CreatedById",
                table: "Banners",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_UpdatedById",
                table: "Banners",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBookings_SportClassId_UserId",
                table: "ClassBookings",
                columns: new[] { "SportClassId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassBookings_UserId",
                table: "ClassBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId_UserId",
                table: "EventRegistrations",
                columns: new[] { "EventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_UserId",
                table: "EventRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SportClasses_CreatedById",
                table: "SportClasses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SportClasses_SportId",
                table: "SportClasses",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_SportClasses_UpdatedById",
                table: "SportClasses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sports_CreatedById",
                table: "Sports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sports_UpdatedById",
                table: "Sports",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "ClassBookings");

            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "SportClasses");

            migrationBuilder.DropTable(
                name: "Sports");

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

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 19, 14, 44, 39, 607, DateTimeKind.Utc).AddTicks(3053));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 19, 14, 44, 39, 607, DateTimeKind.Utc).AddTicks(3058));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 3, 19, 14, 44, 39, 607, DateTimeKind.Utc).AddTicks(3060));
        }
    }
}
