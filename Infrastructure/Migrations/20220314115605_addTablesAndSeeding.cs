using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addTablesAndSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Road",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Road",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Road",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Departments");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Warehouses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankReconStatus",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Organizations",
                type: "nvarchar(200)",
                maxLength: 200,
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

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Departments",
                type: "nvarchar(200)",
                maxLength: 200,
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
                name: "BankAccounts",
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
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Level4_ChAccountId",
                        column: x => x.ChAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Level4_ClearingAccountId",
                        column: x => x.ClearingAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashAccounts",
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
                    table.PrimaryKey("PK_CashAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashAccounts_Level4_ChAccountId",
                        column: x => x.ChAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashAccounts_Transactions_TransactionId",
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

            migrationBuilder.CreateTable(
                name: "BankStmtMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStmtMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankStmtMaster_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankStmtLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<int>(type: "int", nullable: false),
                    StmtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankReconStatus = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStmtLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankStmtLines_BankStmtMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "BankStmtMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankReconciliations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankStmtId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankReconciliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankReconciliations_BankStmtLines_BankStmtId",
                        column: x => x.BankStmtId,
                        principalTable: "BankStmtLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankReconciliations_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Level1",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDelete", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("10000000-5566-7788-99aa-bbccddeeff00"), null, null, false, null, null, "Assets" },
                    { new Guid("20000000-5566-7788-99aa-bbccddeeff00"), null, null, false, null, null, "Liability" },
                    { new Guid("30000000-5566-7788-99aa-bbccddeeff00"), null, null, false, null, null, "Equity" },
                    { new Guid("40000000-5566-7788-99aa-bbccddeeff00"), null, null, false, null, null, "Income" },
                    { new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, false, null, null, "Expenses" }
                });

            migrationBuilder.InsertData(
                table: "Level2",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDelete", "Level1_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), null, null, "Non - Current Assets" },
                    { new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), null, null, "Current Assets" },
                    { new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), null, null, "Non - Current Liabilities" },
                    { new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), null, null, "Current Liabilities" },
                    { new Guid("31000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), null, null, "Owner's Equity" },
                    { new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("40000000-5566-7788-99aa-bbccddeeff00"), null, null, "Income" },
                    { new Guid("51000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Cost of Revenue (COGS)" },
                    { new Guid("52000000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "General & Administrative Expenses" }
                });

            migrationBuilder.InsertData(
                table: "Level3",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDelete", "Level2_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("11100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Fixed Assets" },
                    { new Guid("11200000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Intangible Assets" },
                    { new Guid("11300000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Financial Assets" },
                    { new Guid("11400000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Leased Assets" },
                    { new Guid("12100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Cash & Cash Equivalents" },
                    { new Guid("12200000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Accounts Receivable" },
                    { new Guid("12300000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Prepayments, Advances, deposits" },
                    { new Guid("12400000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Inventory / Merchandise" },
                    { new Guid("21100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Long - term Borrowings" },
                    { new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Accounts Payable" },
                    { new Guid("22200000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Other Liability" },
                    { new Guid("31100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("31000000-5566-7788-99aa-bbccddeeff00"), null, null, "Share Capital" },
                    { new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("31000000-5566-7788-99aa-bbccddeeff00"), null, null, "Reserves" },
                    { new Guid("41100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Operating Income" },
                    { new Guid("41200000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Non - Operating Income" },
                    { new Guid("41300000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Discount" },
                    { new Guid("51100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("51000000-5566-7788-99aa-bbccddeeff00"), null, null, "Direct Cost" },
                    { new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("52000000-5566-7788-99aa-bbccddeeff00"), null, null, "Administrative Expenses" },
                    { new Guid("52200000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("52000000-5566-7788-99aa-bbccddeeff00"), null, null, "Selling, Promotions & Advertising" }
                });

            migrationBuilder.InsertData(
                table: "Level4",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDelete", "Level1_id", "Level3_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("11110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11100000-5566-7788-99aa-bbccddeeff00"), null, null, "Land" },
                    { new Guid("11120000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11100000-5566-7788-99aa-bbccddeeff00"), null, null, "Buildings" },
                    { new Guid("11130000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11100000-5566-7788-99aa-bbccddeeff00"), null, null, "Equipment" },
                    { new Guid("11140000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11100000-5566-7788-99aa-bbccddeeff00"), null, null, "Accumulated Depreciation - Buildings (Contra Asset)" },
                    { new Guid("11410000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11400000-5566-7788-99aa-bbccddeeff00"), null, null, "Leased Building" },
                    { new Guid("11420000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11400000-5566-7788-99aa-bbccddeeff00"), null, null, "Leased Equipment" },
                    { new Guid("11430000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11400000-5566-7788-99aa-bbccddeeff00"), null, null, "Accumulated Depreciation - Leased Building (Contra Asset)" },
                    { new Guid("11440000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("11400000-5566-7788-99aa-bbccddeeff00"), null, null, "Accumulated Depreciation - Leased Equipment (Contra Asset)" },
                    { new Guid("12110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12100000-5566-7788-99aa-bbccddeeff00"), null, null, "Bank" },
                    { new Guid("12120000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12100000-5566-7788-99aa-bbccddeeff00"), null, null, "Cash" },
                    { new Guid("12210000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12200000-5566-7788-99aa-bbccddeeff00"), null, null, "Trade Receivable" },
                    { new Guid("12220000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12200000-5566-7788-99aa-bbccddeeff00"), null, null, "Other Receivable" },
                    { new Guid("12310000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12300000-5566-7788-99aa-bbccddeeff00"), null, null, "Sales Tax Asset" },
                    { new Guid("12320000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12300000-5566-7788-99aa-bbccddeeff00"), null, null, "Income Tax Asset" },
                    { new Guid("12410000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12400000-5566-7788-99aa-bbccddeeff00"), null, null, "Raw Material" },
                    { new Guid("12420000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12400000-5566-7788-99aa-bbccddeeff00"), null, null, "Work in Progress" },
                    { new Guid("12430000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12400000-5566-7788-99aa-bbccddeeff00"), null, null, "Finished Goods" },
                    { new Guid("12440000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), new Guid("12400000-5566-7788-99aa-bbccddeeff00"), null, null, "Goods in Transition" },
                    { new Guid("21110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("21100000-5566-7788-99aa-bbccddeeff00"), null, null, "Long - Term Loans" },
                    { new Guid("21120000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("21100000-5566-7788-99aa-bbccddeeff00"), null, null, "Lease Liability" },
                    { new Guid("22110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, "Bank Over draft" },
                    { new Guid("22120000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, "Trade Accounts Payable" },
                    { new Guid("22130000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, "Accrued Expenses, Loan & Other Payable" },
                    { new Guid("22140000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, "Short Term Lease Payable" },
                    { new Guid("22150000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, "Sales Tax Liability" },
                    { new Guid("22160000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22100000-5566-7788-99aa-bbccddeeff00"), null, null, "Income Tax Liability" },
                    { new Guid("22210000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), new Guid("22200000-5566-7788-99aa-bbccddeeff00"), null, null, "Unearned Revenue" },
                    { new Guid("31110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31100000-5566-7788-99aa-bbccddeeff00"), null, null, "Paid up Share Capital" },
                    { new Guid("31210000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, "Share Premium" },
                    { new Guid("31220000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, "Suplus /Defecit/ Retained Earnings" },
                    { new Guid("31230000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, "Revaluation Reserve" },
                    { new Guid("31240000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, "Exchange Differences" },
                    { new Guid("31250000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, "Drawings" },
                    { new Guid("31260000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("31200000-5566-7788-99aa-bbccddeeff00"), null, null, "Opening Balance equity" },
                    { new Guid("41110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("40000000-5566-7788-99aa-bbccddeeff00"), new Guid("41100000-5566-7788-99aa-bbccddeeff00"), null, null, "Revenue" },
                    { new Guid("41210000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("40000000-5566-7788-99aa-bbccddeeff00"), new Guid("41200000-5566-7788-99aa-bbccddeeff00"), null, null, "Interest Income" },
                    { new Guid("41220000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("40000000-5566-7788-99aa-bbccddeeff00"), new Guid("41200000-5566-7788-99aa-bbccddeeff00"), null, null, "Gain/Loss on Sale of Assets" },
                    { new Guid("41310000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("40000000-5566-7788-99aa-bbccddeeff00"), new Guid("41300000-5566-7788-99aa-bbccddeeff00"), null, null, "Discount" },
                    { new Guid("51110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("51100000-5566-7788-99aa-bbccddeeff00"), null, null, "Direct Labor / Salaries" },
                    { new Guid("51120000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("51100000-5566-7788-99aa-bbccddeeff00"), null, null, "Direct Material" },
                    { new Guid("51130000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("51100000-5566-7788-99aa-bbccddeeff00"), null, null, "Depreciation Expense" },
                    { new Guid("51140000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("51100000-5566-7788-99aa-bbccddeeff00"), null, null, "Cost Of Goods Sold" }
                });

            migrationBuilder.InsertData(
                table: "Level4",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDelete", "Level1_id", "Level3_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("52101000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Interest Expense" },
                    { new Guid("52101100-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Tax Expense" },
                    { new Guid("52110000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Salaries Expense" },
                    { new Guid("52120000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Wages Expense" },
                    { new Guid("52130000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Utilities Expense" },
                    { new Guid("52140000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Entertainment / Meals Expense" },
                    { new Guid("52150000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Printing & Stationary Expense" },
                    { new Guid("52160000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Rent Expense" },
                    { new Guid("52170000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Office Expense" },
                    { new Guid("52180000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Repair & Maintenance Expense" },
                    { new Guid("52190000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52100000-5566-7788-99aa-bbccddeeff00"), null, null, "Transportation & Conveyance Expense" },
                    { new Guid("52210000-5566-7788-99aa-bbccddeeff00"), null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), new Guid("52200000-5566-7788-99aa-bbccddeeff00"), null, null, "Advertising / Marketing Expense" }
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
                name: "IX_BankAccounts_ChAccountId",
                table: "BankAccounts",
                column: "ChAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ClearingAccountId",
                table: "BankAccounts",
                column: "ClearingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_TransactionId",
                table: "BankAccounts",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BankReconciliations_BankStmtId",
                table: "BankReconciliations",
                column: "BankStmtId");

            migrationBuilder.CreateIndex(
                name: "IX_BankReconciliations_PaymentId",
                table: "BankReconciliations",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BankStmtLines_MasterId",
                table: "BankStmtLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BankStmtMaster_BankAccountId",
                table: "BankStmtMaster",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccounts_ChAccountId",
                table: "CashAccounts",
                column: "ChAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccounts_TransactionId",
                table: "CashAccounts",
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
                name: "BankReconciliations");

            migrationBuilder.DropTable(
                name: "CashAccounts");

            migrationBuilder.DropTable(
                name: "RecordLedger");

            migrationBuilder.DropTable(
                name: "BankStmtLines");

            migrationBuilder.DropTable(
                name: "BankStmtMaster");

            migrationBuilder.DropTable(
                name: "BankAccounts");

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

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("11200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("11300000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11120000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11130000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11140000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11410000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11420000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11430000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("11440000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12120000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12210000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12220000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12310000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12320000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12410000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12420000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12430000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("12440000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("21110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("21120000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22120000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22130000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22140000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22150000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22160000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("22210000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31210000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31220000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31230000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31240000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31250000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("31260000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("41110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("41210000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("41220000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("41310000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("51110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("51120000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("51130000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("51140000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52101000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52101100-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52120000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52130000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52140000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52150000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52160000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52170000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52180000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52190000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level4",
                keyColumn: "Id",
                keyValue: new Guid("52210000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("11100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("11400000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("12100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("12200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("12300000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("12400000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("21100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("22100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("22200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("31100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("31200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("41100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("41200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("41300000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("51100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("52100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("52200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("11000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("12000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("21000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("22000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("31000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("41000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("51000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("52000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level1",
                keyColumn: "Id",
                keyValue: new Guid("10000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level1",
                keyColumn: "Id",
                keyValue: new Guid("20000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level1",
                keyColumn: "Id",
                keyValue: new Guid("30000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level1",
                keyColumn: "Id",
                keyValue: new Guid("40000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level1",
                keyColumn: "Id",
                keyValue: new Guid("50000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "BankReconStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "JournalEntryMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "InvoiceMaster");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "CreditNoteMaster");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "BillMaster");

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Road",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "Organizations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Road",
                table: "Organizations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Organizations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Road",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
