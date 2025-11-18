using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class IntegrationMetadataFieldsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "frankfurter",
                column: "timestampjsonfield",
                value: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "frankfurter",
                column: "timestampjsonfield",
                value: "timestamp");
        }
    }
}
