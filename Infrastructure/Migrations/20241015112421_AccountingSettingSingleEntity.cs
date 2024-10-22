using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AccountingSettingSingleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.CreateTable(
                name: "AccountingSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastDay = table.Column<int>(type: "int", nullable: true),
                    ThresholdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DyanmicReports = table.Column<bool>(type: "bit", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    SalesTaxId = table.Column<int>(type: "int", nullable: true),
                    PurchaseTaxId = table.Column<int>(type: "int", nullable: true),
                    Periodicity = table.Column<int>(type: "int", nullable: true),
                    RemindPeriod = table.Column<int>(type: "int", nullable: true),
                    JournalAccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoundPerLine = table.Column<bool>(type: "bit", nullable: true),
                    RoundGlobally = table.Column<bool>(type: "bit", nullable: true),
                    EuropeVAT = table.Column<bool>(type: "bit", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Level4_JournalAccountId",
                        column: x => x.JournalAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Taxes_PurchaseTaxId",
                        column: x => x.PurchaseTaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountingSettings_Taxes_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CountryId",
                table: "AccountingSettings",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_CurrencyId",
                table: "AccountingSettings",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_JournalAccountId",
                table: "AccountingSettings",
                column: "JournalAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_OrganizationId",
                table: "AccountingSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_PurchaseTaxId",
                table: "AccountingSettings",
                column: "PurchaseTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_SalesTaxId",
                table: "AccountingSettings",
                column: "SalesTaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "AccountingSettings");

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
