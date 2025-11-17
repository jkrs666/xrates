using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class TestIntegrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "frankfurter",
                columns: new[] { "freqseconds", "priority" },
                values: new object[] { 10, 1 });

            migrationBuilder.InsertData(
                table: "integrations",
                columns: new[] { "name", "enabled", "freqseconds", "priority", "url" },
                values: new object[,]
                {
                    { "errorTest", true, 10, 0, "invalidUrl" },
                    { "example", true, 10, 0, "https://example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "errorTest");

            migrationBuilder.DeleteData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "example");

            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "frankfurter",
                columns: new[] { "freqseconds", "priority" },
                values: new object[] { 5, 0 });
        }
    }
}
