using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class account_vehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountVehicles_AccountId",
                table: "AccountVehicles",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountVehicles_Accounts_AccountId",
                table: "AccountVehicles",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountVehicles_Accounts_AccountId",
                table: "AccountVehicles");

            migrationBuilder.DropIndex(
                name: "IX_AccountVehicles_AccountId",
                table: "AccountVehicles");
        }
    }
}
