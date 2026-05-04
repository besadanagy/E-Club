using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Club.Migrations
{
    /// <inheritdoc />
    public partial class ImproveEmailFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d7b6b3f1-1ca4-4156-83da-56483c6e1304", new DateTime(2026, 5, 4, 22, 35, 1, 667, DateTimeKind.Utc).AddTicks(5160), "AQAAAAIAAYagAAAAENGJMdYt9grLCJbM3pKUD6Ex4IKOUx0FqZi+dVkUG0Ds3dUk6/ML0ql6RTUG5skUBQ==", "7c5826ce-16b7-4d51-937e-df304fd55143" });

            migrationBuilder.UpdateData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 647, DateTimeKind.Utc).AddTicks(9656));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 657, DateTimeKind.Utc).AddTicks(1432));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 663, DateTimeKind.Utc).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 663, DateTimeKind.Utc).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 663, DateTimeKind.Utc).AddTicks(6938));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 666, DateTimeKind.Utc).AddTicks(2054));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 666, DateTimeKind.Utc).AddTicks(2057));

            migrationBuilder.UpdateData(
                table: "Sports",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2026, 5, 4, 22, 35, 1, 666, DateTimeKind.Utc).AddTicks(2058));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
