using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddBannersAndSportsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SportClasses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SportClasses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SportClasses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sports",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Sports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Sports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SportClasses",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "SportClasses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SportClasses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "SportClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Banners",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Banners",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coaches_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coaches_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "544aa1cd-8dca-4d82-be37-7b46481b166b", new DateTime(2026, 4, 23, 4, 2, 38, 828, DateTimeKind.Utc).AddTicks(5780), "AQAAAAIAAYagAAAAELgR7pF33JwCFf7SJuTplpb0xDmBNjZou9nYVWDbR67SLtQ5pLYo8xVjQp/LaIGDmg==", "bf6cfee8-8927-4a88-97a9-48bbcb2a3b80" });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActionUrl", "CreatedById", "CreatedOn", "Subtitle" },
                values: new object[] { null, null, new DateTime(2026, 4, 23, 4, 2, 38, 809, DateTimeKind.Utc).AddTicks(3212), "Registrations open" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 818, DateTimeKind.Utc).AddTicks(4642));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 825, DateTimeKind.Utc).AddTicks(375));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 825, DateTimeKind.Utc).AddTicks(380));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 825, DateTimeKind.Utc).AddTicks(382));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { null, new DateTime(2026, 4, 23, 4, 2, 38, 827, DateTimeKind.Utc).AddTicks(3431) });

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { null, new DateTime(2026, 4, 23, 4, 2, 38, 827, DateTimeKind.Utc).AddTicks(3434) });

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { null, new DateTime(2026, 4, 23, 4, 2, 38, 827, DateTimeKind.Utc).AddTicks(3436) });

            migrationBuilder.CreateIndex(
                name: "IX_SportClasses_CoachId",
                table: "SportClasses",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_CreatedById",
                table: "Coaches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_UpdatedById",
                table: "Coaches",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SportClasses_Coaches_CoachId",
                table: "SportClasses",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportClasses_Coaches_CoachId",
                table: "SportClasses");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_SportClasses_CoachId",
                table: "SportClasses");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "SportClasses");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sports",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Sports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Sports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SportClasses",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "SportClasses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SportClasses",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Banners",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "Banners",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9aeeebaa-847d-4803-82dc-6fcb6aac9b79", new DateTime(2026, 4, 22, 1, 1, 57, 101, DateTimeKind.Utc).AddTicks(7549), "AQAAAAIAAYagAAAAEBoIoOKR9WTJV5ApNrJPk97+Z4oNnY+e52FAQ8L2vW2B+uiSh3sDwif73kNUdN847g==", "c1e14f18-8fc6-4af6-b4b0-ca9934ba436b" });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActionUrl", "CreatedById", "CreatedOn", "Subtitle" },
                values: new object[] { "/join-tournament", "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Registrations open for the Summer Cup." });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "ActionUrl", "CreatedById", "CreatedOn", "DisplayOrder", "ImageUrl", "IsActive", "Subtitle", "Title", "Type", "UpdatedById", "UpdatedOn" },
                values: new object[,]
                {
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
                table: "SportClasses",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "CurrentParticipants", "Description", "EndTime", "ImageUrl", "Location", "MaxParticipants", "Price", "SportId", "StartTime", "Status", "Title", "Type", "UpdatedById", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, new DateTime(2026, 4, 25, 12, 0, 0, 0, DateTimeKind.Utc), null, "Pitch A", 20, 50m, 1, new DateTime(2026, 4, 25, 10, 0, 0, 0, DateTimeKind.Utc), 0, "Pro Football Drill", 0, null, null },
                    { 2, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, new DateTime(2026, 4, 25, 16, 0, 0, 0, DateTimeKind.Utc), null, "Room 201", 15, 30m, 1, new DateTime(2026, 4, 25, 14, 0, 0, 0, DateTimeKind.Utc), 0, "Tactics & Strategy", 0, null, null },
                    { 3, "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 15, null, new DateTime(2026, 4, 28, 18, 0, 0, 0, DateTimeKind.Utc), null, "Main Stadium", 50, 100m, 1, new DateTime(2026, 4, 28, 9, 0, 0, 0, DateTimeKind.Utc), 0, "Weekend Football Cup", 1, null, null }
                });

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { "22222222-2222-2222-2222-222222222222", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
