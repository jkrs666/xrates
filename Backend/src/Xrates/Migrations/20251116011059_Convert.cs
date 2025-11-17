using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class Convert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "rates",
                columns: new[] { "Id", "Currency", "Timestamp", "Value" },
                values: new object[,]
                {
                    { 14, "EUR", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 0.86057873m },
                    { 15, "CHF", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2.1000m },
                    { 16, "CHF", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2.2000m },
                    { 17, "CHF", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2.3000m },
                    { 18, "CHF", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2.4000m },
                    { 19, "CHF", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 0.79409917m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 19);
        }
    }
}
