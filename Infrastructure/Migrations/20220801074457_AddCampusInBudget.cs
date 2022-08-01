using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddCampusInBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Campuses",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Campuses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NTN",
                table: "Campuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Campuses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SRB",
                table: "Campuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesTaxId",
                table: "Campuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Campuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "BudgetMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetMaster_CampusId",
                table: "BudgetMaster",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetMaster_Campuses_CampusId",
                table: "BudgetMaster",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetMaster_Campuses_CampusId",
                table: "BudgetMaster");

            migrationBuilder.DropIndex(
                name: "IX_BudgetMaster_CampusId",
                table: "BudgetMaster");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "NTN",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "SRB",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "SalesTaxId",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Campuses");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "BudgetMaster");
        }
    }
}
