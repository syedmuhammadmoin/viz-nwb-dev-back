using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddStatusIdColumnInBudgetMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "EstimatedBudgetMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "BudgetMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstimatedBudgetMaster_StatusId",
                table: "EstimatedBudgetMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetMaster_StatusId",
                table: "BudgetMaster",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetMaster_WorkFlowStatus_StatusId",
                table: "BudgetMaster",
                column: "StatusId",
                principalTable: "WorkFlowStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimatedBudgetMaster_WorkFlowStatus_StatusId",
                table: "EstimatedBudgetMaster",
                column: "StatusId",
                principalTable: "WorkFlowStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetMaster_WorkFlowStatus_StatusId",
                table: "BudgetMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_EstimatedBudgetMaster_WorkFlowStatus_StatusId",
                table: "EstimatedBudgetMaster");

            migrationBuilder.DropIndex(
                name: "IX_EstimatedBudgetMaster_StatusId",
                table: "EstimatedBudgetMaster");

            migrationBuilder.DropIndex(
                name: "IX_BudgetMaster_StatusId",
                table: "BudgetMaster");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "EstimatedBudgetMaster");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "BudgetMaster");
        }
    }
}
