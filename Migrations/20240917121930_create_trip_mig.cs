using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class create_trip_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteJson",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TripHeaders");

            migrationBuilder.AddColumn<List<Geometry>>(
                name: "WayPoints",
                table: "Trips",
                type: "geometry[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TripHeaders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TripHeaders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WayPoints",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TripHeaders");

            migrationBuilder.AddColumn<string>(
                name: "RouteJson",
                table: "Trips",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TripHeaders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
