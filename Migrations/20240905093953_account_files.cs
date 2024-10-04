using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class account_files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccountFiles_AccountId",
                table: "AccountFiles",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountFiles_Accounts_AccountId",
                table: "AccountFiles",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountFiles_Accounts_AccountId",
                table: "AccountFiles");

            migrationBuilder.DropIndex(
                name: "IX_AccountFiles_AccountId",
                table: "AccountFiles");
        }
    }
}
