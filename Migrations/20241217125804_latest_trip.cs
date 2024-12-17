using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class latest_trip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripExtraServices_Trips_TripId",
                table: "TripExtraServices");

            migrationBuilder.DropIndex(
                name: "IX_TripExtraServices_TripId",
                table: "TripExtraServices");

            migrationBuilder.DropColumn(
                name: "Ordering",
                table: "WayPoints");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TotalExtraServiceCost",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "TotalExtraServiceCost",
                table: "TripHeaders");

            migrationBuilder.RenameColumn(
                name: "TotalTripCost",
                table: "Trips",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "TotalTripCost",
                table: "TripHeaders",
                newName: "Cost");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Trips",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "Trips",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Route",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Trips",
                newName: "TotalTripCost");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "TripHeaders",
                newName: "TotalTripCost");

            migrationBuilder.AddColumn<int>(
                name: "Ordering",
                table: "WayPoints",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "Trips",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Trips",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExtraServiceCost",
                table: "Trips",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "TripHeaders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TripHeaders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "TripHeaders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExtraServiceCost",
                table: "TripHeaders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TripExtraServices_TripId",
                table: "TripExtraServices",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripExtraServices_Trips_TripId",
                table: "TripExtraServices",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
