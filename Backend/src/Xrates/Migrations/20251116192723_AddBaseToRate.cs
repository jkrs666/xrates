using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseToRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_rates",
                table: "rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_integrations",
                table: "integrations");

            migrationBuilder.RenameTable(
                name: "rates",
                newName: "Rates");

            migrationBuilder.RenameTable(
                name: "integrations",
                newName: "Integrations");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Rates",
                newName: "Quote");

            migrationBuilder.AddColumn<string>(
                name: "Base",
                table: "Rates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Integrations",
                table: "Integrations",
                column: "Name");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 1,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 2,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 4,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 5,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 6,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 7,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 8,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 9,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 10,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 11,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 12,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 13,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 14,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 15,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 16,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 17,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 18,
                column: "Base",
                value: "USD");

            migrationBuilder.UpdateData(
                table: "Rates",
                keyColumn: "Id",
                keyValue: 19,
                column: "Base",
                value: "USD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Integrations",
                table: "Integrations");

            migrationBuilder.DropColumn(
                name: "Base",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "rates");

            migrationBuilder.RenameTable(
                name: "Integrations",
                newName: "integrations");

            migrationBuilder.RenameColumn(
                name: "Quote",
                table: "rates",
                newName: "Currency");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rates",
                table: "rates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_integrations",
                table: "integrations",
                column: "Name");
        }
    }
}
