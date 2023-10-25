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
            migrationBuilder.DropForeignKey(
                name: "FK_tripTransports_Currencies_CurrencyId",
                table: "tripTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_tripTransports_TransportModes_TransportModeId",
                table: "tripTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_tripTransports_Trips_TripId",
                table: "tripTransports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tripTransports",
                table: "tripTransports");

            migrationBuilder.RenameTable(
                name: "tripTransports",
                newName: "TripTransports");

            migrationBuilder.RenameIndex(
                name: "IX_tripTransports_TransportModeId",
                table: "TripTransports",
                newName: "IX_TripTransports_TransportModeId");

            migrationBuilder.RenameIndex(
                name: "IX_tripTransports_CurrencyId",
                table: "TripTransports",
                newName: "IX_TripTransports_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripTransports",
                table: "TripTransports",
                columns: new[] { "TripId", "TransportModeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TripTransports_Currencies_CurrencyId",
                table: "TripTransports",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTransports_TransportModes_TransportModeId",
                table: "TripTransports",
                column: "TransportModeId",
                principalTable: "TransportModes",
                principalColumn: "TransportModeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripTransports_Trips_TripId",
                table: "TripTransports",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTransports_Currencies_CurrencyId",
                table: "TripTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_TripTransports_TransportModes_TransportModeId",
                table: "TripTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_TripTransports_Trips_TripId",
                table: "TripTransports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripTransports",
                table: "TripTransports");

            migrationBuilder.RenameTable(
                name: "TripTransports",
                newName: "tripTransports");

            migrationBuilder.RenameIndex(
                name: "IX_TripTransports_TransportModeId",
                table: "tripTransports",
                newName: "IX_tripTransports_TransportModeId");

            migrationBuilder.RenameIndex(
                name: "IX_TripTransports_CurrencyId",
                table: "tripTransports",
                newName: "IX_tripTransports_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tripTransports",
                table: "tripTransports",
                columns: new[] { "TripId", "TransportModeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_tripTransports_Currencies_CurrencyId",
                table: "tripTransports",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_tripTransports_TransportModes_TransportModeId",
                table: "tripTransports",
                column: "TransportModeId",
                principalTable: "TransportModes",
                principalColumn: "TransportModeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tripTransports_Trips_TripId",
                table: "tripTransports",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
