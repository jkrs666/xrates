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
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnixTs = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Base = table.Column<int>(type: "integer", nullable: false),
                    Quote = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Rates",
                columns: new[] { "Id", "Base", "Quote", "UnixTs", "Value" },
                values: new object[,]
                {
                    { 1, 150, 46, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.1111m },
                    { 2, 150, 46, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1.2111m },
                    { 3, 150, 46, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1.3111m },
                    { 4, 150, 46, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1.4111m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");
        }
    }
}
