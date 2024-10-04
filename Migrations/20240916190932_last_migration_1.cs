using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class last_migration_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountLocations_Accounts_AccountId1",
                table: "AccountLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountLocations",
                table: "AccountLocations");

            migrationBuilder.DropIndex(
                name: "IX_AccountLocations_AccountId1",
                table: "AccountLocations");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AccountLocations");

            migrationBuilder.RenameColumn(
                name: "AccountId1",
                table: "AccountLocations",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "TripHeaders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLocationUpdateTime",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Accounts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Accounts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AccountLocations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AccountLocations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AccountLocations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AccountLocations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccountLocations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountLocations",
                table: "AccountLocations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLocations_AccountId",
                table: "AccountLocations",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLocations_Accounts_AccountId",
                table: "AccountLocations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountLocations_Accounts_AccountId",
                table: "AccountLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountLocations",
                table: "AccountLocations");

            migrationBuilder.DropIndex(
                name: "IX_AccountLocations_AccountId",
                table: "AccountLocations");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "LastLocationUpdateTime",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AccountLocations");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AccountLocations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AccountLocations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AccountLocations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccountLocations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AccountLocations",
                newName: "AccountId1");

            migrationBuilder.AddColumn<Geometry>(
                name: "Location",
                table: "AccountLocations",
                type: "geometry",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountLocations",
                table: "AccountLocations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLocations_AccountId1",
                table: "AccountLocations",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountLocations_Accounts_AccountId1",
                table: "AccountLocations",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
