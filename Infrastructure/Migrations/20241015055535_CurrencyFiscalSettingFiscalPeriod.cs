using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class CurrencyFiscalSettingFiscalPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.AddColumn<bool>(
                name: "IsCubicFeetVol",
                table: "UnitOfMeasurement",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCubicMeterVol",
                table: "UnitOfMeasurement",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsKilogram",
                table: "UnitOfMeasurement",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPound",
                table: "UnitOfMeasurement",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "UnitOfMeasurement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrencySettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencySettings_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencySettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiscalPeriods_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriodSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastDay = table.Column<int>(type: "int", nullable: true),
                    ThresholdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DyanmicReports = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriodSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_OrganizationId",
                table: "UnitOfMeasurement",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencySettings_CurrencyId",
                table: "CurrencySettings",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencySettings_OrganizationId",
                table: "CurrencySettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriods_OrganizationId",
                table: "FiscalPeriods",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitOfMeasurement_Organizations_OrganizationId",
                table: "UnitOfMeasurement",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitOfMeasurement_Organizations_OrganizationId",
                table: "UnitOfMeasurement");

            migrationBuilder.DropTable(
                name: "CurrencySettings");

            migrationBuilder.DropTable(
                name: "FiscalPeriods");

            migrationBuilder.DropTable(
                name: "FiscalPeriodSettings");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_OrganizationId",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "IsCubicFeetVol",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "IsCubicMeterVol",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "IsKilogram",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "IsPound",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "UnitOfMeasurement");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
