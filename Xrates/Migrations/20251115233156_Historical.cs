using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class Historical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: 1.1011m);

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Timestamp", "Value" },
                values: new object[] { new DateTime(2025, 1, 1, 2, 0, 0, 0, DateTimeKind.Utc), 1.1211m });

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Timestamp", "Value" },
                values: new object[] { new DateTime(2025, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1.1111m });

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Timestamp", "Value" },
                values: new object[] { new DateTime(2025, 1, 2, 3, 0, 0, 0, DateTimeKind.Utc), 1.2311m });

            migrationBuilder.InsertData(
                table: "rates",
                columns: new[] { "Id", "Currency", "Timestamp", "Value" },
                values: new object[,]
                {
                    { 5, "EUR", new DateTime(2025, 1, 2, 2, 0, 0, 0, DateTimeKind.Utc), 1.2211m },
                    { 6, "EUR", new DateTime(2025, 1, 2, 4, 0, 0, 0, DateTimeKind.Utc), 1.2411m },
                    { 7, "EUR", new DateTime(2025, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc), 1.2111m },
                    { 8, "EUR", new DateTime(2025, 1, 3, 1, 0, 0, 0, DateTimeKind.Utc), 1.3111m },
                    { 9, "EUR", new DateTime(2025, 1, 3, 2, 0, 0, 0, DateTimeKind.Utc), 1.3211m },
                    { 10, "EUR", new DateTime(2025, 1, 3, 3, 0, 0, 0, DateTimeKind.Utc), 1.3311m },
                    { 11, "EUR", new DateTime(2025, 1, 4, 3, 0, 0, 0, DateTimeKind.Utc), 1.4311m },
                    { 12, "EUR", new DateTime(2025, 1, 4, 2, 0, 0, 0, DateTimeKind.Utc), 1.4211m },
                    { 13, "EUR", new DateTime(2025, 1, 4, 1, 0, 0, 0, DateTimeKind.Utc), 1.4111m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: 1.1111m);

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Timestamp", "Value" },
                values: new object[] { new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1.2111m });

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Timestamp", "Value" },
                values: new object[] { new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1.3111m });

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Timestamp", "Value" },
                values: new object[] { new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1.4111m });
        }
    }
}
