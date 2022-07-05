using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddGRNToBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GRNId",
                table: "BillMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_GRNId",
                table: "BillMaster",
                column: "GRNId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillMaster_GRNMaster_GRNId",
                table: "BillMaster",
                column: "GRNId",
                principalTable: "GRNMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillMaster_GRNMaster_GRNId",
                table: "BillMaster");

            migrationBuilder.DropIndex(
                name: "IX_BillMaster_GRNId",
                table: "BillMaster");

            migrationBuilder.DropColumn(
                name: "GRNId",
                table: "BillMaster");
        }
    }
}
