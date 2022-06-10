using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addDocumentLedgerIdInPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentLedgerId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentLedgerId",
                table: "DebitNoteMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentLedgerId",
                table: "CreditNoteMaster",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentLedgerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DocumentLedgerId",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "DocumentLedgerId",
                table: "CreditNoteMaster");
        }
    }
}
