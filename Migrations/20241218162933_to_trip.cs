using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class to_trip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoundsId",
                table: "Trips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TripDirection",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TripBound",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartLatitude = table.Column<double>(type: "double precision", nullable: false),
                    StartLongitude = table.Column<double>(type: "double precision", nullable: false),
                    EndLatitude = table.Column<double>(type: "double precision", nullable: false),
                    EndLongitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripBound", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BoundsId",
                table: "Trips",
                column: "BoundsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripBound_BoundsId",
                table: "Trips",
                column: "BoundsId",
                principalTable: "TripBound",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripBound_BoundsId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "TripBound");

            migrationBuilder.DropIndex(
                name: "IX_Trips_BoundsId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "BoundsId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripDirection",
                table: "Trips");
        }
    }
}
