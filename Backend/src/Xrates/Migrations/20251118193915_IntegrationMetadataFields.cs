using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class IntegrationMetadataFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "basecurrency",
                table: "integrations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ratesjsonfield",
                table: "integrations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "timestampjsonfield",
                table: "integrations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "errorTest",
                columns: new[] { "basecurrency", "ratesjsonfield", "timestampjsonfield" },
                values: new object[] { "USD", "rates", "date" });

            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "example",
                columns: new[] { "basecurrency", "ratesjsonfield", "timestampjsonfield" },
                values: new object[] { "USD", "rates", "timestamp" });

            migrationBuilder.UpdateData(
                table: "integrations",
                keyColumn: "name",
                keyValue: "frankfurter",
                columns: new[] { "basecurrency", "ratesjsonfield", "timestampjsonfield" },
                values: new object[] { "USD", "rates", "timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "basecurrency",
                table: "integrations");

            migrationBuilder.DropColumn(
                name: "ratesjsonfield",
                table: "integrations");

            migrationBuilder.DropColumn(
                name: "timestampjsonfield",
                table: "integrations");
        }
    }
}
