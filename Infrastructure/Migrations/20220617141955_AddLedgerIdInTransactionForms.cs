using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddLedgerIdInTransactionForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "PayrollTransactionMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "InvoiceMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "DebitNoteMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "CreditNoteMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "BillMaster",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "InvoiceMaster");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "CreditNoteMaster");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "BillMaster");
        }
    }
}
