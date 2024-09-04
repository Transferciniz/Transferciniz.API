using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class company_sessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Users_UserId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_TripHeaders_Transactions_TransactionId",
                table: "TripHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "VehicleLocations");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TripHeaders_TransactionId",
                table: "TripHeaders");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChildCount",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "HandicapedPersonCount",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "PersonCount",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "TripHeaders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Sessions",
                newName: "RelatedId");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Trips",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TripHeaders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TripHeaders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SessionType",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Geometry>(
                name: "Location",
                table: "CompanyVehicles",
                type: "geometry",
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                table: "Companies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "Companies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "UserMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMemberships", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyVehicles_CompanyId",
                table: "CompanyVehicles",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyVehicles_Companies_CompanyId",
                table: "CompanyVehicles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyVehicles_Companies_CompanyId",
                table: "CompanyVehicles");

            migrationBuilder.DropTable(
                name: "UserMemberships");

            migrationBuilder.DropIndex(
                name: "IX_CompanyVehicles_CompanyId",
                table: "CompanyVehicles");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TripHeaders");

            migrationBuilder.DropColumn(
                name: "SessionType",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "CompanyVehicles");

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "RelatedId",
                table: "Sessions",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Vehicles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Trips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildCount",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HandicapedPersonCount",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonCount",
                table: "Trips",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "TripHeaders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "VehicleLocations",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Geometry>(type: "geometry", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLocations", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_VehicleLocations_Vehicles_VehicleId1",
                        column: x => x.VehicleId1,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TripHeaders_TransactionId",
                table: "TripHeaders",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLocations_VehicleId1",
                table: "VehicleLocations",
                column: "VehicleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Users_UserId",
                table: "Sessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripHeaders_Transactions_TransactionId",
                table: "TripHeaders",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
