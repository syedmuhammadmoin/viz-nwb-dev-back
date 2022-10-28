using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddOtherTaxAndTaxInDebitNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OtherTax",
                table: "DebitNoteMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "DebitNoteMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AnyOtherTax",
                table: "DebitNoteLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherTax",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "AnyOtherTax",
                table: "DebitNoteLines");
        }
    }
}
