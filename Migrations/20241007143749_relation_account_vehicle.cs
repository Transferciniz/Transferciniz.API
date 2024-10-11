using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class relation_account_vehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyVehicleId",
                table: "Trips",
                newName: "AccountVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_AccountVehicleId",
                table: "Trips",
                column: "AccountVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_AccountVehicles_AccountVehicleId",
                table: "Trips",
                column: "AccountVehicleId",
                principalTable: "AccountVehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_AccountVehicles_AccountVehicleId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_AccountVehicleId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "AccountVehicleId",
                table: "Trips",
                newName: "CompanyVehicleId");
        }
    }
}
