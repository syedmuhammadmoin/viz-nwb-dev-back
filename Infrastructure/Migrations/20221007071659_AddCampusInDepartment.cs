using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddCampusInDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Campuses_CampusId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CampusId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CampusId",
                table: "Departments",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Campuses_CampusId",
                table: "Departments",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Campuses_CampusId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CampusId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CampusId",
                table: "Employees",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Campuses_CampusId",
                table: "Employees",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
