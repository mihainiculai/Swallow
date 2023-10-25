using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swallow.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CountryId",
                table: "Currencies",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CountryId",
                table: "Currencies",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Countries_CountryId",
                table: "Currencies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Countries_CountryId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_CountryId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Currencies");
        }
    }
}
