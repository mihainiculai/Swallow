using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicUsername",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 17, 15, 39, 58, 73, DateTimeKind.Utc).AddTicks(7901));

            migrationBuilder.CreateIndex(
                name: "IX_Users_PublicUsername",
                table: "Users",
                column: "PublicUsername",
                unique: true,
                filter: "[PublicUsername] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PublicUsername",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PublicUsername",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2023, 12, 5, 10, 46, 22, 177, DateTimeKind.Utc).AddTicks(2373));
        }
    }
}
