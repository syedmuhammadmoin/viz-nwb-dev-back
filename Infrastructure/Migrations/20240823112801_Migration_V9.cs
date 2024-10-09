using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Migration_V9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Level4_Level4_id",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "CashAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "DefaultAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "LossAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "ProfitAccount",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "SuspenseAccount",
                table: "Journals");

            migrationBuilder.RenameColumn(
                name: "Level4_id",
                table: "Journals",
                newName: "SuspenseAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Journals_Level4_id",
                table: "Journals",
                newName: "IX_Journals_SuspenseAccountId");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumberId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CashAccountId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultAccountId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LossAccountId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfitAccountId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journals_AccountNumberId",
                table: "Journals",
                column: "AccountNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_CashAccountId",
                table: "Journals",
                column: "CashAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_DefaultAccountId",
                table: "Journals",
                column: "DefaultAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_LossAccountId",
                table: "Journals",
                column: "LossAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_ProfitAccountId",
                table: "Journals",
                column: "ProfitAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_AccountNumberId",
                table: "Journals",
                column: "AccountNumberId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_CashAccountId",
                table: "Journals",
                column: "CashAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_DefaultAccountId",
                table: "Journals",
                column: "DefaultAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_LossAccountId",
                table: "Journals",
                column: "LossAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_ProfitAccountId",
                table: "Journals",
                column: "ProfitAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_SuspenseAccountId",
                table: "Journals",
                column: "SuspenseAccountId",
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
                name: "FK_Journals_Level4_AccountNumberId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Level4_CashAccountId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Level4_DefaultAccountId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Level4_LossAccountId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Level4_ProfitAccountId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_Journals_Level4_SuspenseAccountId",
                table: "Journals");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Journals_AccountNumberId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_CashAccountId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_DefaultAccountId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_LossAccountId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_ProfitAccountId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "AccountNumberId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "CashAccountId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "DefaultAccountId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "LossAccountId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "ProfitAccountId",
                table: "Journals");

            migrationBuilder.RenameColumn(
                name: "SuspenseAccountId",
                table: "Journals",
                newName: "Level4_id");

            migrationBuilder.RenameIndex(
                name: "IX_Journals_SuspenseAccountId",
                table: "Journals",
                newName: "IX_Journals_Level4_id");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
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
                name: "DefaultAccount",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LossAccount",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_Level4_Level4_id",
                table: "Journals",
                column: "Level4_id",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
