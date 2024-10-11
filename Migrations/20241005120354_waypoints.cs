using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class waypoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WayPoints",
                table: "Trips");

            migrationBuilder.CreateTable(
                name: "Waypoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waypoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Waypoints_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WayPointUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WayPointId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WayPointUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WayPointUsers_Waypoints_WayPointId",
                        column: x => x.WayPointId,
                        principalTable: "Waypoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Waypoints_TripId",
                table: "Waypoints",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_WayPointUsers_WayPointId",
                table: "WayPointUsers",
                column: "WayPointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WayPointUsers");

            migrationBuilder.DropTable(
                name: "Waypoints");

            migrationBuilder.AddColumn<List<Geometry>>(
                name: "WayPoints",
                table: "Trips",
                type: "geometry[]",
                nullable: false);
        }
    }
}
