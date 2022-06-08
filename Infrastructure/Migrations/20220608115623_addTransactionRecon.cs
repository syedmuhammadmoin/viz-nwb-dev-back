using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addTransactionRecon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionReconciles_Transactions_DocumentTransactionId",
                table: "TransactionReconciles");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionReconciles_Transactions_PaymentTransactionId",
                table: "TransactionReconciles");

            migrationBuilder.RenameColumn(
                name: "PaymentTransactionId",
                table: "TransactionReconciles",
                newName: "PaymentLedgerId");

            migrationBuilder.RenameColumn(
                name: "DocumentTransactionId",
                table: "TransactionReconciles",
                newName: "DocumentLegderId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionReconciles_PaymentTransactionId",
                table: "TransactionReconciles",
                newName: "IX_TransactionReconciles_PaymentLedgerId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionReconciles_DocumentTransactionId",
                table: "TransactionReconciles",
                newName: "IX_TransactionReconciles_DocumentLegderId");

            migrationBuilder.AddColumn<bool>(
                name: "IsReconcilable",
                table: "RecordLedger",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReconStatus",
                table: "RecordLedger",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionReconciles_RecordLedger_DocumentLegderId",
                table: "TransactionReconciles",
                column: "DocumentLegderId",
                principalTable: "RecordLedger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionReconciles_RecordLedger_PaymentLedgerId",
                table: "TransactionReconciles",
                column: "PaymentLedgerId",
                principalTable: "RecordLedger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionReconciles_RecordLedger_DocumentLegderId",
                table: "TransactionReconciles");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionReconciles_RecordLedger_PaymentLedgerId",
                table: "TransactionReconciles");

            migrationBuilder.DropColumn(
                name: "IsReconcilable",
                table: "RecordLedger");

            migrationBuilder.DropColumn(
                name: "ReconStatus",
                table: "RecordLedger");

            migrationBuilder.RenameColumn(
                name: "PaymentLedgerId",
                table: "TransactionReconciles",
                newName: "PaymentTransactionId");

            migrationBuilder.RenameColumn(
                name: "DocumentLegderId",
                table: "TransactionReconciles",
                newName: "DocumentTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionReconciles_PaymentLedgerId",
                table: "TransactionReconciles",
                newName: "IX_TransactionReconciles_PaymentTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionReconciles_DocumentLegderId",
                table: "TransactionReconciles",
                newName: "IX_TransactionReconciles_DocumentTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionReconciles_Transactions_DocumentTransactionId",
                table: "TransactionReconciles",
                column: "DocumentTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionReconciles_Transactions_PaymentTransactionId",
                table: "TransactionReconciles",
                column: "PaymentTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
