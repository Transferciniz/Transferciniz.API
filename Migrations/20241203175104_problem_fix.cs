using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class problem_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "AccountVehicleProblems",
                newName: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountVehicleProblems_AccountId",
                table: "AccountVehicleProblems",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountVehicleProblems_Accounts_AccountId",
                table: "AccountVehicleProblems",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountVehicleProblems_Accounts_AccountId",
                table: "AccountVehicleProblems");

            migrationBuilder.DropIndex(
                name: "IX_AccountVehicleProblems_AccountId",
                table: "AccountVehicleProblems");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "AccountVehicleProblems",
                newName: "DriverId");
        }
    }
}
