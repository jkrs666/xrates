using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xrates.Migrations
{
    /// <inheritdoc />
    public partial class RatePairIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_rates_base_quote",
                table: "rates",
                columns: new[] { "base", "quote" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_rates_base_quote",
                table: "rates");
        }
    }
}
