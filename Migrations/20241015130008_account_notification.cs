using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class account_notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountNotifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WayPointUsers_AccountId",
                table: "WayPointUsers",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_WayPointUsers_Accounts_AccountId",
                table: "WayPointUsers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WayPointUsers_Accounts_AccountId",
                table: "WayPointUsers");

            migrationBuilder.DropTable(
                name: "AccountNotifications");

            migrationBuilder.DropIndex(
                name: "IX_WayPointUsers_AccountId",
                table: "WayPointUsers");
        }
    }
}
