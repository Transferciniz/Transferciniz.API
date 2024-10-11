using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class vehicle_last_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "AccountVehicles");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AccountVehicles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AccountVehicles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AccountVehicles");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AccountVehicles");

            migrationBuilder.AddColumn<Geometry>(
                name: "Location",
                table: "AccountVehicles",
                type: "geometry",
                nullable: false);
        }
    }
}
