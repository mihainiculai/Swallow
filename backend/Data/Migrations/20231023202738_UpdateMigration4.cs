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
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Currencies_CurrencyId",
                table: "Attractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Countries_CountryId",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRate_Currencies_CurrencyId",
                table: "CurrencyRate");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Currencies_CurrencyId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryAttractions_Currencies_CurrencyId",
                table: "ItineraryAttractions");

            migrationBuilder.DropForeignKey(
                name: "FK_TripTransports_Currencies_CurrencyId",
                table: "TripTransports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currency");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_CountryId",
                table: "Currency",
                newName: "IX_Currency_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Currency_CurrencyId",
                table: "Attractions",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_Countries_CountryId",
                table: "Currency",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRate_Currency_CurrencyId",
                table: "CurrencyRate",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Currency_CurrencyId",
                table: "Expenses",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryAttractions_Currency_CurrencyId",
                table: "ItineraryAttractions",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTransports_Currency_CurrencyId",
                table: "TripTransports",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Currency_CurrencyId",
                table: "Attractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Currency_Countries_CountryId",
                table: "Currency");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRate_Currency_CurrencyId",
                table: "CurrencyRate");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Currency_CurrencyId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryAttractions_Currency_CurrencyId",
                table: "ItineraryAttractions");

            migrationBuilder.DropForeignKey(
                name: "FK_TripTransports_Currency_CurrencyId",
                table: "TripTransports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.RenameTable(
                name: "Currency",
                newName: "Currencies");

            migrationBuilder.RenameIndex(
                name: "IX_Currency_CountryId",
                table: "Currencies",
                newName: "IX_Currencies_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Currencies_CurrencyId",
                table: "Attractions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Countries_CountryId",
                table: "Currencies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRate_Currencies_CurrencyId",
                table: "CurrencyRate",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Currencies_CurrencyId",
                table: "Expenses",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryAttractions_Currencies_CurrencyId",
                table: "ItineraryAttractions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTransports_Currencies_CurrencyId",
                table: "TripTransports",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");
        }
    }
}
