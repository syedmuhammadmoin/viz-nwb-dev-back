using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Migration_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Journals",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankAccountId",
                table: "Journals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAcountId",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Journals",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CashAccount",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LossAccount",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Journals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProfitAccount",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuspenseAccount",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journals_BankAccountId",
                table: "Journals",
                column: "BankAccountId",
                unique: true,
                filter: "[BankAccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_OrganizationId",
                table: "Journals",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_BankAccounts_BankAccountId",
                table: "Journals",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Organizations_OrganizationId",
                table: "Journals",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Journals_BankAccounts_BankAccountId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Organizations_OrganizationId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Journals_BankAccountId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_OrganizationId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "BankAcountId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "CashAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "LossAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "ProfitAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "SuspenseAccount",
                table: "Journals");

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
