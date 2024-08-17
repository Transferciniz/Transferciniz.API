using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypeFilters_VehicleTypeId",
                table: "VehicleTypeFilters",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSegmentFilters_VehicleSegmentId",
                table: "VehicleSegmentFilters",
                column: "VehicleSegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleExtraServices_ExtraServiceId",
                table: "VehicleExtraServices",
                column: "ExtraServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleExtraServices_ExtraServices_ExtraServiceId",
                table: "VehicleExtraServices",
                column: "ExtraServiceId",
                principalTable: "ExtraServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleSegmentFilters_VehicleSegments_VehicleSegmentId",
                table: "VehicleSegmentFilters",
                column: "VehicleSegmentId",
                principalTable: "VehicleSegments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypeFilters_VehicleTypes_VehicleTypeId",
                table: "VehicleTypeFilters",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleExtraServices_ExtraServices_ExtraServiceId",
                table: "VehicleExtraServices");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSegmentFilters_VehicleSegments_VehicleSegmentId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypeFilters_VehicleTypes_VehicleTypeId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypeFilters_VehicleTypeId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleSegmentFilters_VehicleSegmentId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropIndex(
                name: "IX_VehicleExtraServices_ExtraServiceId",
                table: "VehicleExtraServices");
        }
    }
}
