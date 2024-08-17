using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class vehicleLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VehicleId1",
                table: "VehicleLocations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLocations_VehicleId1",
                table: "VehicleLocations",
                column: "VehicleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLocations_Vehicles_VehicleId1",
                table: "VehicleLocations",
                column: "VehicleId1",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLocations_Vehicles_VehicleId1",
                table: "VehicleLocations");

            migrationBuilder.DropIndex(
                name: "IX_VehicleLocations_VehicleId1",
                table: "VehicleLocations");

            migrationBuilder.DropColumn(
                name: "VehicleId1",
                table: "VehicleLocations");
        }
    }
}
