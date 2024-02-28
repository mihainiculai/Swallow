using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GooglePlaceId",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 28, 19, 26, 19, 7, DateTimeKind.Utc).AddTicks(6058));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GooglePlaceId",
                table: "Attractions",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 27, 14, 15, 9, 560, DateTimeKind.Utc).AddTicks(5151));
        }
    }
}
