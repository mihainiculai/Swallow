using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformSettings",
                columns: table => new
                {
                    SettingsId = table.Column<byte>(type: "tinyint", nullable: false),
                    MentenanceMode = table.Column<bool>(type: "bit", nullable: false),
                    NextCurrencyUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSettings", x => x.SettingsId);
                });

            migrationBuilder.InsertData(
                table: "PlatformSettings",
                columns: new[] { "SettingsId", "MentenanceMode", "NextCurrencyUpdate" },
                values: new object[] { (byte)1, false, new DateTime(2023, 10, 25, 11, 43, 8, 401, DateTimeKind.Utc).AddTicks(4135) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformSettings");
        }
    }
}
