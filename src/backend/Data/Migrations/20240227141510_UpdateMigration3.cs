using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.data.migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Attractions",
                type: "decimal(3,1)",
                precision: 3,
                scale: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRatingsTotal",
                table: "Attractions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 27, 14, 15, 9, 560, DateTimeKind.Utc).AddTicks(5151));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "UserRatingsTotal",
                table: "Attractions");

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 26, 10, 48, 5, 857, DateTimeKind.Utc).AddTicks(2171));
        }
    }
}
