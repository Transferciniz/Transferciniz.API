using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class new_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Companies_CompanyId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Transactions_TransactionId1",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Vehicles_VehicleId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleExtraServices_Vehicles_VehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleFiles_Vehicles_VehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSegmentFilters_Vehicles_VehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypeFilters_Vehicles_VehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypeFilters_VehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleSegmentFilters_VehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleFiles_VehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropIndex(
                name: "IX_VehicleExtraServices_VehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CompanyId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TransactionId1",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_UserId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Plate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Trips",
                newName: "TripHeaderId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Trips",
                newName: "DriverId");

            migrationBuilder.RenameColumn(
                name: "TransactionId1",
                table: "Trips",
                newName: "CompanyVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_VehicleId",
                table: "Trips",
                newName: "IX_Trips_TripHeaderId");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyVehicleId",
                table: "VehicleTypeFilters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyVehicleId",
                table: "VehicleSegmentFilters",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Vehicles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "Vehicles",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleBrandId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleModelId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyVehicleId",
                table: "VehicleFiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyVehicleId",
                table: "VehicleExtraServices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plate = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalExtraServiceCost = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalTripCost = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false),
                    Fee = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripHeaders_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    ExtraCapacity = table.Column<int>(type: "integer", nullable: false),
                    VehicleBrandId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleModels_VehicleBrands_VehicleBrandId",
                        column: x => x.VehicleBrandId,
                        principalTable: "VehicleBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeFilters_CompanyVehicleId",
                table: "VehicleTypeFilters",
                column: "CompanyVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSegmentFilters_CompanyVehicleId",
                table: "VehicleSegmentFilters",
                column: "CompanyVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleBrandId",
                table: "Vehicles",
                column: "VehicleBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleModelId",
                table: "Vehicles",
                column: "VehicleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleFiles_CompanyVehicleId",
                table: "VehicleFiles",
                column: "CompanyVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleExtraServices_CompanyVehicleId",
                table: "VehicleExtraServices",
                column: "CompanyVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyVehicles_VehicleId",
                table: "CompanyVehicles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TripHeaders_TransactionId",
                table: "TripHeaders",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_VehicleBrandId",
                table: "VehicleModels",
                column: "VehicleBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripHeaders_TripHeaderId",
                table: "Trips",
                column: "TripHeaderId",
                principalTable: "TripHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleExtraServices_CompanyVehicles_CompanyVehicleId",
                table: "VehicleExtraServices",
                column: "CompanyVehicleId",
                principalTable: "CompanyVehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleFiles_CompanyVehicles_CompanyVehicleId",
                table: "VehicleFiles",
                column: "CompanyVehicleId",
                principalTable: "CompanyVehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleBrands_VehicleBrandId",
                table: "Vehicles",
                column: "VehicleBrandId",
                principalTable: "VehicleBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleModels_VehicleModelId",
                table: "Vehicles",
                column: "VehicleModelId",
                principalTable: "VehicleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleSegmentFilters_CompanyVehicles_CompanyVehicleId",
                table: "VehicleSegmentFilters",
                column: "CompanyVehicleId",
                principalTable: "CompanyVehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypeFilters_CompanyVehicles_CompanyVehicleId",
                table: "VehicleTypeFilters",
                column: "CompanyVehicleId",
                principalTable: "CompanyVehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripHeaders_TripHeaderId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleExtraServices_CompanyVehicles_CompanyVehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleFiles_CompanyVehicles_CompanyVehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleBrands_VehicleBrandId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleModels_VehicleModelId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSegmentFilters_CompanyVehicles_CompanyVehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypeFilters_CompanyVehicles_CompanyVehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropTable(
                name: "CompanyVehicles");

            migrationBuilder.DropTable(
                name: "TripHeaders");

            migrationBuilder.DropTable(
                name: "VehicleModels");

            migrationBuilder.DropTable(
                name: "VehicleBrands");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypeFilters_CompanyVehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleSegmentFilters_CompanyVehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleBrandId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleModelId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_VehicleFiles_CompanyVehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropIndex(
                name: "IX_VehicleExtraServices_CompanyVehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.DropColumn(
                name: "CompanyVehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropColumn(
                name: "CompanyVehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleBrandId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleModelId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CompanyVehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropColumn(
                name: "CompanyVehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.RenameColumn(
                name: "TripHeaderId",
                table: "Trips",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "Trips",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CompanyVehicleId",
                table: "Trips",
                newName: "TransactionId1");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_TripHeaderId",
                table: "Trips",
                newName: "IX_Trips_VehicleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plate",
                table: "Vehicles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Trips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Trips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Trips",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeFilters_VehicleId",
                table: "VehicleTypeFilters",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSegmentFilters_VehicleId",
                table: "VehicleSegmentFilters",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleFiles_VehicleId",
                table: "VehicleFiles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleExtraServices_VehicleId",
                table: "VehicleExtraServices",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CompanyId",
                table: "Trips",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TransactionId1",
                table: "Trips",
                column: "TransactionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Companies_CompanyId",
                table: "Trips",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Transactions_TransactionId1",
                table: "Trips",
                column: "TransactionId1",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_UserId",
                table: "Trips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Vehicles_VehicleId",
                table: "Trips",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleExtraServices_Vehicles_VehicleId",
                table: "VehicleExtraServices",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleFiles_Vehicles_VehicleId",
                table: "VehicleFiles",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleSegmentFilters_Vehicles_VehicleId",
                table: "VehicleSegmentFilters",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypeFilters_Vehicles_VehicleId",
                table: "VehicleTypeFilters",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
