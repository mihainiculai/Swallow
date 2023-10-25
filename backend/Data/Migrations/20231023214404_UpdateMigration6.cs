using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRate_Currencies_CurrencyId",
                table: "CurrencyRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRate",
                table: "CurrencyRate");

            migrationBuilder.RenameTable(
                name: "CurrencyRate",
                newName: "CurrencyRates");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRate_CurrencyId",
                table: "CurrencyRates",
                newName: "IX_CurrencyRates_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRates",
                table: "CurrencyRates",
                column: "CurrencyRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_Currencies_CurrencyId",
                table: "CurrencyRates",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_Currencies_CurrencyId",
                table: "CurrencyRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRates",
                table: "CurrencyRates");

            migrationBuilder.RenameTable(
                name: "CurrencyRates",
                newName: "CurrencyRate");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRates_CurrencyId",
                table: "CurrencyRate",
                newName: "IX_CurrencyRate_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRate",
                table: "CurrencyRate",
                column: "CurrencyRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRate_Currencies_CurrencyId",
                table: "CurrencyRate",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
