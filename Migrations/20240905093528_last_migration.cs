using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Transferciniz.API.Migrations
{
    /// <inheritdoc />
    public partial class last_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleExtraServices_CompanyVehicles_CompanyVehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleFiles_CompanyVehicles_CompanyVehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSegmentFilters_CompanyVehicles_CompanyVehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypeFilters_CompanyVehicles_CompanyVehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropTable(
                name: "CompanyFiles");

            migrationBuilder.DropTable(
                name: "CompanyVehicles");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "UserFiles");

            migrationBuilder.DropTable(
                name: "UserLocations");

            migrationBuilder.DropTable(
                name: "UserMemberships");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "SessionType",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "CompanyVehicleId",
                table: "VehicleTypeFilters",
                newName: "AccountVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleTypeFilters_CompanyVehicleId",
                table: "VehicleTypeFilters",
                newName: "IX_VehicleTypeFilters_AccountVehicleId");

            migrationBuilder.RenameColumn(
                name: "CompanyVehicleId",
                table: "VehicleSegmentFilters",
                newName: "AccountVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleSegmentFilters_CompanyVehicleId",
                table: "VehicleSegmentFilters",
                newName: "IX_VehicleSegmentFilters_AccountVehicleId");

            migrationBuilder.RenameColumn(
                name: "CompanyVehicleId",
                table: "VehicleFiles",
                newName: "AccountVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleFiles_CompanyVehicleId",
                table: "VehicleFiles",
                newName: "IX_VehicleFiles_AccountVehicleId");

            migrationBuilder.RenameColumn(
                name: "CompanyVehicleId",
                table: "VehicleExtraServices",
                newName: "AccountVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleExtraServices_CompanyVehicleId",
                table: "VehicleExtraServices",
                newName: "IX_VehicleExtraServices_AccountVehicleId");

            migrationBuilder.RenameColumn(
                name: "RelatedId",
                table: "Sessions",
                newName: "AccountId");

            migrationBuilder.CreateTable(
                name: "AccountFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    File = table.Column<byte[]>(type: "bytea", nullable: false),
                    FileCategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountMemberships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: false),
                    AccountType = table.Column<int>(type: "integer", nullable: false),
                    TaxNumber = table.Column<string>(type: "text", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "numeric", nullable: false),
                    InvoiceAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plate = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Geometry>(type: "geometry", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountLocations",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Geometry>(type: "geometry", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLocations", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_AccountLocations_Accounts_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AccountId",
                table: "Sessions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLocations_AccountId1",
                table: "AccountLocations",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_AccountVehicles_VehicleId",
                table: "AccountVehicles",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Accounts_AccountId",
                table: "Sessions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleExtraServices_AccountVehicles_AccountVehicleId",
                table: "VehicleExtraServices",
                column: "AccountVehicleId",
                principalTable: "AccountVehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleFiles_AccountVehicles_AccountVehicleId",
                table: "VehicleFiles",
                column: "AccountVehicleId",
                principalTable: "AccountVehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleSegmentFilters_AccountVehicles_AccountVehicleId",
                table: "VehicleSegmentFilters",
                column: "AccountVehicleId",
                principalTable: "AccountVehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypeFilters_AccountVehicles_AccountVehicleId",
                table: "VehicleTypeFilters",
                column: "AccountVehicleId",
                principalTable: "AccountVehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Accounts_AccountId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleExtraServices_AccountVehicles_AccountVehicleId",
                table: "VehicleExtraServices");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleFiles_AccountVehicles_AccountVehicleId",
                table: "VehicleFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleSegmentFilters_AccountVehicles_AccountVehicleId",
                table: "VehicleSegmentFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypeFilters_AccountVehicles_AccountVehicleId",
                table: "VehicleTypeFilters");

            migrationBuilder.DropTable(
                name: "AccountFiles");

            migrationBuilder.DropTable(
                name: "AccountLocations");

            migrationBuilder.DropTable(
                name: "AccountMemberships");

            migrationBuilder.DropTable(
                name: "AccountVehicles");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_AccountId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "AccountVehicleId",
                table: "VehicleTypeFilters",
                newName: "CompanyVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleTypeFilters_AccountVehicleId",
                table: "VehicleTypeFilters",
                newName: "IX_VehicleTypeFilters_CompanyVehicleId");

            migrationBuilder.RenameColumn(
                name: "AccountVehicleId",
                table: "VehicleSegmentFilters",
                newName: "CompanyVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleSegmentFilters_AccountVehicleId",
                table: "VehicleSegmentFilters",
                newName: "IX_VehicleSegmentFilters_CompanyVehicleId");

            migrationBuilder.RenameColumn(
                name: "AccountVehicleId",
                table: "VehicleFiles",
                newName: "CompanyVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleFiles_AccountVehicleId",
                table: "VehicleFiles",
                newName: "IX_VehicleFiles_CompanyVehicleId");

            migrationBuilder.RenameColumn(
                name: "AccountVehicleId",
                table: "VehicleExtraServices",
                newName: "CompanyVehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleExtraServices_AccountVehicleId",
                table: "VehicleExtraServices",
                newName: "IX_VehicleExtraServices_CompanyVehicleId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Sessions",
                newName: "RelatedId");

            migrationBuilder.AddColumn<int>(
                name: "SessionType",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CompanyType = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: false),
                    TaxNumber = table.Column<string>(type: "text", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMemberships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    File = table.Column<byte[]>(type: "bytea", nullable: false),
                    FileCategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyFiles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Geometry>(type: "geometry", nullable: false),
                    Plate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyVehicles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationVersion = table.Column<string>(type: "text", nullable: false),
                    DeviceInfo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDevices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    File = table.Column<byte[]>(type: "bytea", nullable: false),
                    FileCategory = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLocations",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<Geometry>(type: "geometry", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocations", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserLocations_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFiles_CompanyId",
                table: "CompanyFiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyVehicles_CompanyId",
                table: "CompanyVehicles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyVehicles_VehicleId",
                table: "CompanyVehicles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_UserId1",
                table: "UserLocations",
                column: "UserId1");

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
    }
}
