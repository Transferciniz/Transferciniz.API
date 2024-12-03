using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class vehicle_problems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountVehicleProblems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountVehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountVehicleProblems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountVehicleProblems_AccountVehicles_AccountVehicleId",
                        column: x => x.AccountVehicleId,
                        principalTable: "AccountVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountVehicleProblemHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountVehicleProblemId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<int>(type: "integer", nullable: false),
                    ToStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountVehicleProblemHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountVehicleProblemHistories_AccountVehicleProblems_Accou~",
                        column: x => x.AccountVehicleProblemId,
                        principalTable: "AccountVehicleProblems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountVehicleProblemHistories_AccountVehicleProblemId",
                table: "AccountVehicleProblemHistories",
                column: "AccountVehicleProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountVehicleProblems_AccountVehicleId",
                table: "AccountVehicleProblems",
                column: "AccountVehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountVehicleProblemHistories");

            migrationBuilder.DropTable(
                name: "AccountVehicleProblems");
        }
    }
}
