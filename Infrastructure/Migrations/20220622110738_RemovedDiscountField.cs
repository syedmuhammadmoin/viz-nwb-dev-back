using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemovedDiscountField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "Taxes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "SRB Tax Asset");

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "AccountId", "CreatedBy", "CreatedDate", "IsDelete", "ModifiedBy", "ModifiedDate", "Name", "TaxType" },
                values: new object[] { 6, null, null, null, false, null, null, "SRB Tax Liability", 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Taxes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Taxes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "SRB");
        }
    }
}
