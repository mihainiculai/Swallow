using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RateToUSD",
                table: "CurrencyRates",
                type: "decimal(12,6)",
                precision: 12,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,6)",
                oldPrecision: 10,
                oldScale: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RateToUSD",
                table: "CurrencyRates",
                type: "decimal(10,6)",
                precision: 10,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldPrecision: 12,
                oldScale: 6);
        }
    }
}
