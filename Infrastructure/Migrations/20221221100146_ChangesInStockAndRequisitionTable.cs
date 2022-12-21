using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ChangesInStockAndRequisitionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservedRequisitionQuantity",
                table: "Stock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithoutWorkflow",
                table: "RequisitionMaster",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "RequisitionMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "RequisitionLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReserveQuantity",
                table: "RequisitionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedRequisitionQuantity",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "IsWithoutWorkflow",
                table: "RequisitionMaster");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "RequisitionMaster");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "RequisitionLines");

            migrationBuilder.DropColumn(
                name: "ReserveQuantity",
                table: "RequisitionLines");
        }
    }
}
