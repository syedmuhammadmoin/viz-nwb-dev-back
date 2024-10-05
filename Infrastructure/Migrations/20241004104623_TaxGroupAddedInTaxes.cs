using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TaxGroupAddedInTaxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Taxes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_GroupId",
                table: "Taxes",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_TaxGroups_GroupId",
                table: "Taxes",
                column: "GroupId",
                principalTable: "TaxGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_TaxGroups_GroupId",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_GroupId",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Taxes");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
