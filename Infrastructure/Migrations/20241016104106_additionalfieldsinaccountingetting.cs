using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class additionalfieldsinaccountingetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.AddColumn<bool>(
                name: "AddressValidation",
                table: "AccountingSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiId",
                table: "AccountingSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "AccountingSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseTaxReceivedAccountId",
                table: "AccountingSettings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CommitTransactions",
                table: "AccountingSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "AccountingSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "AccountingSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCashBasisId",
                table: "AccountingSettings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseUPC",
                table: "AccountingSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_BaseTaxReceivedAccountId",
                table: "AccountingSettings",
                column: "BaseTaxReceivedAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingSettings_TaxCashBasisId",
                table: "AccountingSettings",
                column: "TaxCashBasisId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Level4_BaseTaxReceivedAccountId",
                table: "AccountingSettings",
                column: "BaseTaxReceivedAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingSettings_Level4_TaxCashBasisId",
                table: "AccountingSettings",
                column: "TaxCashBasisId",
                principalTable: "Level4",
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
                name: "FK_AccountingSettings_Level4_BaseTaxReceivedAccountId",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingSettings_Level4_TaxCashBasisId",
                table: "AccountingSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_BaseTaxReceivedAccountId",
                table: "AccountingSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountingSettings_TaxCashBasisId",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "AddressValidation",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ApiId",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "BaseTaxReceivedAccountId",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "CommitTransactions",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "Environment",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "TaxCashBasisId",
                table: "AccountingSettings");

            migrationBuilder.DropColumn(
                name: "UseUPC",
                table: "AccountingSettings");

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
