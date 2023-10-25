using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "CurrencyRates",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2023, 10, 25, 12, 1, 27, 688, DateTimeKind.Utc).AddTicks(7551));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "CurrencyRates",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2023, 10, 25, 11, 43, 8, 401, DateTimeKind.Utc).AddTicks(4135));
        }
    }
}
