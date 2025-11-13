using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "integrations",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_integrations", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "integrations",
                columns: new[] { "Name", "Url" },
                values: new object[] { "frankfurter", "https://api.frankfurter.dev/v1/latest?base=USD" });

            migrationBuilder.InsertData(
                table: "rates",
                columns: new[] { "Id", "Currency", "Timestamp", "Value" },
                values: new object[,]
                {
                    { 1, "EUR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.1111m },
                    { 2, "EUR", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1.2111m },
                    { 3, "EUR", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1.3111m },
                    { 4, "EUR", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1.4111m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "integrations");

            migrationBuilder.DropTable(
                name: "rates");
        }
    }
}
