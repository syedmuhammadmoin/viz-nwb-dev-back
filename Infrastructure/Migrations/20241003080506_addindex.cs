using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyLine_CurrencyId",
                table: "CurrencyLine");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyLine_Date",
                table: "CurrencyLine");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyLine_CurrencyId_Date",
                table: "CurrencyLine",
                columns: new[] { "CurrencyId", "Date" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyLine_CurrencyId_Date",
                table: "CurrencyLine");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyLine_CurrencyId",
                table: "CurrencyLine",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyLine_Date",
                table: "CurrencyLine",
                column: "Date",
                unique: true);

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
