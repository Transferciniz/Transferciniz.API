using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class problem_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountVehicleProblemHistories_AccountId",
                table: "AccountVehicleProblemHistories",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountVehicleProblemHistories_Accounts_AccountId",
                table: "AccountVehicleProblemHistories",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountVehicleProblemHistories_Accounts_AccountId",
                table: "AccountVehicleProblemHistories");

            migrationBuilder.DropIndex(
                name: "IX_AccountVehicleProblemHistories_AccountId",
                table: "AccountVehicleProblemHistories");
        }
    }
}
