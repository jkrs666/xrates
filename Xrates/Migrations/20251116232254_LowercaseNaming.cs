using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class LowercaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Integrations",
                table: "Integrations");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "rates");

            migrationBuilder.RenameTable(
                name: "Integrations",
                newName: "integrations");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "rates",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "rates",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Quote",
                table: "rates",
                newName: "quote");

            migrationBuilder.RenameColumn(
                name: "Base",
                table: "rates",
                newName: "base");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "rates",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "integrations",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "FreqSeconds",
                table: "integrations",
                newName: "freqseconds");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "integrations",
                newName: "name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_rates",
                table: "rates",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_integrations",
                table: "integrations",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_rates",
                table: "rates");

            migrationBuilder.DropPrimaryKey(
                name: "pk_integrations",
                table: "integrations");

            migrationBuilder.RenameTable(
                name: "rates",
                newName: "Rates");

            migrationBuilder.RenameTable(
                name: "integrations",
                newName: "Integrations");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Rates",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Rates",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "quote",
                table: "Rates",
                newName: "Quote");

            migrationBuilder.RenameColumn(
                name: "base",
                table: "Rates",
                newName: "Base");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Rates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Integrations",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "freqseconds",
                table: "Integrations",
                newName: "FreqSeconds");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Integrations",
                newName: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Integrations",
                table: "Integrations",
                column: "Name");
        }
    }
}
