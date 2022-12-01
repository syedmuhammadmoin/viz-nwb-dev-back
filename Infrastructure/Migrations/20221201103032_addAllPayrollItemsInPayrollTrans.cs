using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addAllPayrollItemsInPayrollTrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BPSAmount",
                table: "PayrollTransactionMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IncrementAmount",
                table: "PayrollTransactionMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "IncrementName",
                table: "PayrollTransactionMaster",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "PayrollTransactionLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_BPSAccountId",
                table: "PayrollTransactionMaster",
                column: "BPSAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollTransactionMaster_Level4_BPSAccountId",
                table: "PayrollTransactionMaster",
                column: "BPSAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollTransactionMaster_Level4_BPSAccountId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropIndex(
                name: "IX_PayrollTransactionMaster_BPSAccountId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "BPSAmount",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "IncrementAmount",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "IncrementName",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PayrollTransactionLines");
        }
    }
}
