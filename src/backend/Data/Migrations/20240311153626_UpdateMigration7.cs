using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "OpenTime",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 3, 11, 15, 36, 26, 461, DateTimeKind.Utc).AddTicks(4548));

            migrationBuilder.InsertData(
                table: "Weekdays",
                columns: new[] { "WeekdayId", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Monday" },
                    { (byte)2, "Tuesday" },
                    { (byte)3, "Wednesday" },
                    { (byte)4, "Thursday" },
                    { (byte)5, "Friday" },
                    { (byte)6, "Saturday" },
                    { (byte)7, "Sunday" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)4);

            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)5);

            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)6);

            migrationBuilder.DeleteData(
                table: "Weekdays",
                keyColumn: "WeekdayId",
                keyValue: (byte)7);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "OpenTime",
                table: "Schedules",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "SettingsId",
                keyValue: (byte)1,
                column: "NextCurrencyUpdate",
                value: new DateTime(2024, 3, 1, 13, 38, 55, 579, DateTimeKind.Utc).AddTicks(1545));
        }
    }
}
