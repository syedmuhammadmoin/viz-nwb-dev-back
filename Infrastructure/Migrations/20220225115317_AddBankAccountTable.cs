using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddBankAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "JournalEntryMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "InvoiceMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "DebitNoteMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "CreditNoteMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "BillMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DocType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ChAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClearingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount_Level4_ChAccountId",
                        column: x => x.ChAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccount_Level4_ClearingAccountId",
                        column: x => x.ClearingAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccount_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashAccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Handler = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ChAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashAccount_Level4_ChAccountId",
                        column: x => x.ChAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashAccount_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecordLedger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Level4_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sign = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLedger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordLedger_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLedger_Level4_Level4_id",
                        column: x => x.Level4_id,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLedger_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLedger_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryMaster_TransactionId",
                table: "JournalEntryMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_TransactionId",
                table: "InvoiceMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_TransactionId",
                table: "DebitNoteMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_TransactionId",
                table: "CreditNoteMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_TransactionId",
                table: "BillMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_ChAccountId",
                table: "BankAccount",
                column: "ChAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_ClearingAccountId",
                table: "BankAccount",
                column: "ClearingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_TransactionId",
                table: "BankAccount",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccount_ChAccountId",
                table: "CashAccount",
                column: "ChAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccount_TransactionId",
                table: "CashAccount",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_BusinessPartnerId",
                table: "RecordLedger",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_Level4_id",
                table: "RecordLedger",
                column: "Level4_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_LocationId",
                table: "RecordLedger",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_TransactionId",
                table: "RecordLedger",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillMaster_Transactions_TransactionId",
                table: "BillMaster",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNoteMaster_Transactions_TransactionId",
                table: "CreditNoteMaster",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNoteMaster_Transactions_TransactionId",
                table: "DebitNoteMaster",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceMaster_Transactions_TransactionId",
                table: "InvoiceMaster",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntryMaster_Transactions_TransactionId",
                table: "JournalEntryMaster",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Transactions_TransactionId",
                table: "Payments",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillMaster_Transactions_TransactionId",
                table: "BillMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditNoteMaster_Transactions_TransactionId",
                table: "CreditNoteMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNoteMaster_Transactions_TransactionId",
                table: "DebitNoteMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceMaster_Transactions_TransactionId",
                table: "InvoiceMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntryMaster_Transactions_TransactionId",
                table: "JournalEntryMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Transactions_TransactionId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "BankAccount");

            migrationBuilder.DropTable(
                name: "CashAccount");

            migrationBuilder.DropTable(
                name: "RecordLedger");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryMaster_TransactionId",
                table: "JournalEntryMaster");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceMaster_TransactionId",
                table: "InvoiceMaster");

            migrationBuilder.DropIndex(
                name: "IX_DebitNoteMaster_TransactionId",
                table: "DebitNoteMaster");

            migrationBuilder.DropIndex(
                name: "IX_CreditNoteMaster_TransactionId",
                table: "CreditNoteMaster");

            migrationBuilder.DropIndex(
                name: "IX_BillMaster_TransactionId",
                table: "BillMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "JournalEntryMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "InvoiceMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "CreditNoteMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "BillMaster");
        }
    }
}
