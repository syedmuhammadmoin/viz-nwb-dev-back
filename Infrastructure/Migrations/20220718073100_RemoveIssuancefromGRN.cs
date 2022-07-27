using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemoveIssuancefromGRN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNMaster_IssuanceMaster_IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropIndex(
                name: "IX_GRNMaster_IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropColumn(
                name: "IssuanceId",
                table: "GRNMaster");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "GRNMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "GRNMaster",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IssuanceId",
                table: "GRNMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaster_IssuanceId",
                table: "GRNMaster",
                column: "IssuanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNMaster_IssuanceMaster_IssuanceId",
                table: "GRNMaster",
                column: "IssuanceId",
                principalTable: "IssuanceMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
