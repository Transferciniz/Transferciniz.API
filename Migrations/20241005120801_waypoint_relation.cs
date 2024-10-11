using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class waypoint_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Waypoints_Trips_TripId",
                table: "Waypoints");

            migrationBuilder.DropForeignKey(
                name: "FK_WayPointUsers_Waypoints_WayPointId",
                table: "WayPointUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Waypoints",
                table: "Waypoints");

            migrationBuilder.RenameTable(
                name: "Waypoints",
                newName: "WayPoints");

            migrationBuilder.RenameIndex(
                name: "IX_Waypoints_TripId",
                table: "WayPoints",
                newName: "IX_WayPoints_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WayPoints",
                table: "WayPoints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WayPoints_Trips_TripId",
                table: "WayPoints",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WayPointUsers_WayPoints_WayPointId",
                table: "WayPointUsers",
                column: "WayPointId",
                principalTable: "WayPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WayPoints_Trips_TripId",
                table: "WayPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_WayPointUsers_WayPoints_WayPointId",
                table: "WayPointUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WayPoints",
                table: "WayPoints");

            migrationBuilder.RenameTable(
                name: "WayPoints",
                newName: "Waypoints");

            migrationBuilder.RenameIndex(
                name: "IX_WayPoints_TripId",
                table: "Waypoints",
                newName: "IX_Waypoints_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Waypoints",
                table: "Waypoints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Waypoints_Trips_TripId",
                table: "Waypoints",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WayPointUsers_Waypoints_WayPointId",
                table: "WayPointUsers",
                column: "WayPointId",
                principalTable: "Waypoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
