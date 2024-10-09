using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class JournalTypeChang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenTaxes_Taxes_TaxId",
                table: "ChildrenTaxes");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.CreateTable(
                name: "TaxSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_TaxSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxSetting_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxSetting_Level4_JournalAccountId",
                        column: x => x.JournalAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxSetting_Taxes_PurchaseTaxId",
                        column: x => x.PurchaseTaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxSetting_Taxes_SalesTaxId",
                        column: x => x.SalesTaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxSetting_CountryId",
                table: "TaxSetting",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxSetting_JournalAccountId",
                table: "TaxSetting",
                column: "JournalAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxSetting_PurchaseTaxId",
                table: "TaxSetting",
                column: "PurchaseTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxSetting_SalesTaxId",
                table: "TaxSetting",
                column: "SalesTaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenTaxes_Taxes_TaxId",
                table: "ChildrenTaxes",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_ChildrenTaxes_Taxes_TaxId",
                table: "ChildrenTaxes");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "CurrencyLine");

            migrationBuilder.DropTable(
                name: "TaxSetting");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenTaxes_Taxes_TaxId",
                table: "ChildrenTaxes",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
