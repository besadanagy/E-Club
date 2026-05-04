using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class AddCoaches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportClasses_Coaches_CoachId",
                table: "SportClasses");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "Coaches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Coaches",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Coaches",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Coaches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "006142ad-95b5-4bbb-928d-b5608a0fb379", new DateTime(2026, 4, 26, 23, 32, 4, 677, DateTimeKind.Utc).AddTicks(7658), "AQAAAAIAAYagAAAAEGLPAxvq2cmf4I2GJCEPKpjpimEYKEoEB3QRsg5jQDazEDz8e+w3MfpvhyV2QCpiLA==", "1de6847f-cc31-4be8-9bcf-ab3247fa1aff" });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 651, DateTimeKind.Utc).AddTicks(8762));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 662, DateTimeKind.Utc).AddTicks(6424));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 671, DateTimeKind.Utc).AddTicks(4623));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 671, DateTimeKind.Utc).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 671, DateTimeKind.Utc).AddTicks(4634));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 674, DateTimeKind.Utc).AddTicks(8701));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 674, DateTimeKind.Utc).AddTicks(8706));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 26, 23, 32, 4, 674, DateTimeKind.Utc).AddTicks(8708));

            migrationBuilder.AddForeignKey(
                name: "FK_SportClasses_Coaches_CoachId",
                table: "SportClasses",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportClasses_Coaches_CoachId",
                table: "SportClasses");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

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
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 809, DateTimeKind.Utc).AddTicks(3212));

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
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 827, DateTimeKind.Utc).AddTicks(3431));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 827, DateTimeKind.Utc).AddTicks(3434));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 4, 23, 4, 2, 38, 827, DateTimeKind.Utc).AddTicks(3436));

            migrationBuilder.AddForeignKey(
                name: "FK_SportClasses_Coaches_CoachId",
                table: "SportClasses",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }
    }
}
