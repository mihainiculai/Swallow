using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "RateToUSD",
                table: "Currencies");

            migrationBuilder.CreateTable(
                name: "CurrencyRate",
                columns: table => new
                {
                    CurrencyRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<short>(type: "smallint", nullable: false),
                    RateToUSD = table.Column<decimal>(type: "decimal(10,6)", precision: 10, scale: 6, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRate", x => x.CurrencyRateId);
                    table.ForeignKey(
                        name: "FK_CurrencyRate_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRate_CurrencyId",
                table: "CurrencyRate",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastUpdate",
                table: "Currencies",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateToUSD",
                table: "Currencies",
                type: "decimal(10,6)",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
