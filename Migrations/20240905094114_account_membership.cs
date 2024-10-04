using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class account_membership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountMemberships_AccountId",
                table: "AccountMemberships",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountMemberships_Accounts_AccountId",
                table: "AccountMemberships",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountMemberships_Accounts_AccountId",
                table: "AccountMemberships");

            migrationBuilder.DropIndex(
                name: "IX_AccountMemberships_AccountId",
                table: "AccountMemberships");
        }
    }
}
