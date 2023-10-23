using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemoveCampusIdColBudgetReappropriationLines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetReappropriationLines_Campuses_CampusId",
                table: "BudgetReappropriationLines");

            migrationBuilder.DropIndex(
                name: "IX_BudgetReappropriationLines_CampusId",
                table: "BudgetReappropriationLines");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "BudgetReappropriationLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "BudgetReappropriationLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetReappropriationLines_CampusId",
                table: "BudgetReappropriationLines",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetReappropriationLines_Campuses_CampusId",
                table: "BudgetReappropriationLines",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
