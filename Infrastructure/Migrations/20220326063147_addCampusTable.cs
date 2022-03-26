using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addCampusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillLines_Locations_LocationId",
                table: "BillLines");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditNoteLines_Locations_LocationId",
                table: "CreditNoteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNoteLines_Locations_LocationId",
                table: "DebitNoteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Locations_LocationId",
                table: "InvoiceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntryLines_Locations_LocationId",
                table: "JournalEntryLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLedger_Locations_LocationId",
                table: "RecordLedger");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Departments_DepartmentId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryLines_LocationId",
                table: "JournalEntryLines");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "PurchasedOrSold",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "JournalEntryLines");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "CashAccounts");

            migrationBuilder.DropColumn(
                name: "City",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Entity",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "State",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Warehouses",
                newName: "CampusId");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_DepartmentId",
                table: "Warehouses",
                newName: "IX_Warehouses_CampusId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "RecordLedger",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_RecordLedger_LocationId",
                table: "RecordLedger",
                newName: "IX_RecordLedger_WarehouseId");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Products",
                newName: "PurchasePrice");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "InvoiceLines",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceLines_LocationId",
                table: "InvoiceLines",
                newName: "IX_InvoiceLines_WarehouseId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "DebitNoteLines",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_DebitNoteLines_LocationId",
                table: "DebitNoteLines",
                newName: "IX_DebitNoteLines_WarehouseId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "CreditNoteLines",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_CreditNoteLines_LocationId",
                table: "CreditNoteLines",
                newName: "IX_CreditNoteLines_WarehouseId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "BillLines",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_BillLines_LocationId",
                table: "BillLines",
                newName: "IX_BillLines_WarehouseId");

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "RecordLedger",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "JournalEntryMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "JournalEntryLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "InvoiceMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "DebitNoteMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "CreditNoteMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "CashAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpeningBalanceDate",
                table: "CashAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "BillMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "BankAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Campuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campuses_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_CampusId",
                table: "RecordLedger",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CampusId",
                table: "Payments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryMaster_CampusId",
                table: "JournalEntryMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_WarehouseId",
                table: "JournalEntryLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_CampusId",
                table: "InvoiceMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_CampusId",
                table: "DebitNoteMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_CampusId",
                table: "CreditNoteMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccounts_CampusId",
                table: "CashAccounts",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_CampusId",
                table: "BillMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CampusId",
                table: "BankAccounts",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_OrganizationId",
                table: "Campuses",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Campuses_CampusId",
                table: "BankAccounts",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillLines_Warehouses_WarehouseId",
                table: "BillLines",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillMaster_Campuses_CampusId",
                table: "BillMaster",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashAccounts_Campuses_CampusId",
                table: "CashAccounts",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNoteLines_Warehouses_WarehouseId",
                table: "CreditNoteLines",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNoteMaster_Campuses_CampusId",
                table: "CreditNoteMaster",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNoteLines_Warehouses_WarehouseId",
                table: "DebitNoteLines",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNoteMaster_Campuses_CampusId",
                table: "DebitNoteMaster",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Warehouses_WarehouseId",
                table: "InvoiceLines",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceMaster_Campuses_CampusId",
                table: "InvoiceMaster",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntryLines_Warehouses_WarehouseId",
                table: "JournalEntryLines",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntryMaster_Campuses_CampusId",
                table: "JournalEntryMaster",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Campuses_CampusId",
                table: "Payments",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLedger_Campuses_CampusId",
                table: "RecordLedger",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLedger_Warehouses_WarehouseId",
                table: "RecordLedger",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Campuses_CampusId",
                table: "Warehouses",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Campuses_CampusId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BillLines_Warehouses_WarehouseId",
                table: "BillLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BillMaster_Campuses_CampusId",
                table: "BillMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_CashAccounts_Campuses_CampusId",
                table: "CashAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditNoteLines_Warehouses_WarehouseId",
                table: "CreditNoteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditNoteMaster_Campuses_CampusId",
                table: "CreditNoteMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNoteLines_Warehouses_WarehouseId",
                table: "DebitNoteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNoteMaster_Campuses_CampusId",
                table: "DebitNoteMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_Warehouses_WarehouseId",
                table: "InvoiceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceMaster_Campuses_CampusId",
                table: "InvoiceMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntryLines_Warehouses_WarehouseId",
                table: "JournalEntryLines");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntryMaster_Campuses_CampusId",
                table: "JournalEntryMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Campuses_CampusId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLedger_Campuses_CampusId",
                table: "RecordLedger");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLedger_Warehouses_WarehouseId",
                table: "RecordLedger");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Campuses_CampusId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropIndex(
                name: "IX_RecordLedger_CampusId",
                table: "RecordLedger");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CampusId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryMaster_CampusId",
                table: "JournalEntryMaster");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryLines_WarehouseId",
                table: "JournalEntryLines");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceMaster_CampusId",
                table: "InvoiceMaster");

            migrationBuilder.DropIndex(
                name: "IX_DebitNoteMaster_CampusId",
                table: "DebitNoteMaster");

            migrationBuilder.DropIndex(
                name: "IX_CreditNoteMaster_CampusId",
                table: "CreditNoteMaster");

            migrationBuilder.DropIndex(
                name: "IX_CashAccounts_CampusId",
                table: "CashAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BillMaster_CampusId",
                table: "BillMaster");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_CampusId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "RecordLedger");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "JournalEntryMaster");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "JournalEntryLines");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "InvoiceMaster");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "DebitNoteMaster");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "CreditNoteMaster");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "CashAccounts");

            migrationBuilder.DropColumn(
                name: "OpeningBalanceDate",
                table: "CashAccounts");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "BillMaster");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "CampusId",
                table: "Warehouses",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_CampusId",
                table: "Warehouses",
                newName: "IX_Warehouses_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "RecordLedger",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_RecordLedger_WarehouseId",
                table: "RecordLedger",
                newName: "IX_RecordLedger_LocationId");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "Products",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "InvoiceLines",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceLines_WarehouseId",
                table: "InvoiceLines",
                newName: "IX_InvoiceLines_LocationId");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "DebitNoteLines",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_DebitNoteLines_WarehouseId",
                table: "DebitNoteLines",
                newName: "IX_DebitNoteLines_LocationId");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "CreditNoteLines",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_CreditNoteLines_WarehouseId",
                table: "CreditNoteLines",
                newName: "IX_CreditNoteLines_LocationId");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "BillLines",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_BillLines_WarehouseId",
                table: "BillLines",
                newName: "IX_BillLines_LocationId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Warehouses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Warehouses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchasedOrSold",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "JournalEntryLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "CashAccounts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "BusinessPartners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "BusinessPartners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Entity",
                table: "BusinessPartners",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "BusinessPartners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "BankAccounts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeadOfDept = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dimensions = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Supervisor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_LocationId",
                table: "JournalEntryLines",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_WarehouseId",
                table: "Locations",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillLines_Locations_LocationId",
                table: "BillLines",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNoteLines_Locations_LocationId",
                table: "CreditNoteLines",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNoteLines_Locations_LocationId",
                table: "DebitNoteLines",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_Locations_LocationId",
                table: "InvoiceLines",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntryLines_Locations_LocationId",
                table: "JournalEntryLines",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLedger_Locations_LocationId",
                table: "RecordLedger",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Departments_DepartmentId",
                table: "Warehouses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
