using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddTaxFormTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    TaxType = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxes_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "AccountId", "CreatedBy", "CreatedDate", "IsDelete", "ModifiedBy", "ModifiedDate", "Name", "TaxType" },
                values: new object[,]
                {
                    { 1, null, null, null, false, null, null, "Sales Tax Asset", 0 },
                    { 2, null, null, null, false, null, null, "Sales Tax Liability", 1 },
                    { 3, null, null, null, false, null, null, "Income Tax Asset", 2 },
                    { 4, null, null, null, false, null, null, "Income Tax Liability", 3 },
                    { 5, null, null, null, false, null, null, "SRB", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_AccountId",
                table: "Taxes",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taxes");
        }
    }
}
