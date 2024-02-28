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
            migrationBuilder.RenameColumn(
                name: "TripAdvisorURL",
                table: "Attractions",
                newName: "TripAdvisorUrl");

            migrationBuilder.RenameColumn(
                name: "PictureURL",
                table: "Attractions",
                newName: "PictureUrl");

            migrationBuilder.RenameColumn(
                name: "GoogleMapsURL",
                table: "Attractions",
                newName: "GoogleMapsUrl");

            migrationBuilder.AddColumn<string>(
                name: "VisitDuration",
                table: "Attractions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 26, 10, 48, 5, 857, DateTimeKind.Utc).AddTicks(2171));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitDuration",
                table: "Attractions");

            migrationBuilder.RenameColumn(
                name: "TripAdvisorUrl",
                table: "Attractions",
                newName: "TripAdvisorURL");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "Attractions",
                newName: "PictureURL");

            migrationBuilder.RenameColumn(
                name: "GoogleMapsUrl",
                table: "Attractions",
                newName: "GoogleMapsURL");

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 2, 26, 6, 18, 56, 494, DateTimeKind.Utc).AddTicks(8659));
        }
    }
}
