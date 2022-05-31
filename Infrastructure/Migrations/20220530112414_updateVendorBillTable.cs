using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class updateVendorBillTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Manager",
                table: "Warehouses",
                newName: "StoreManager");

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "RecordLedger",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "SRBTax",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountReceivableId",
                table: "BusinessPartners",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountPayableId",
                table: "BusinessPartners",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "OtherTax",
                table: "BillMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "BillMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AnyOtherTax",
                table: "BillLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BankAccountType",
                table: "BankAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "BankAccounts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstimatedBudgetMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimatedBudgetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimatedBudgetMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimatedBudgetMaster_BudgetMaster_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "BudgetMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayrollType = table.Column<int>(type: "int", nullable: false),
                    PayrollItemType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollItems_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PODate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    RequisitionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitionMaster_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Domicile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Maritalstatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlaceofBirth = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DateofJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateofRetirment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EarnedLeaves = table.Column<int>(type: "int", nullable: false),
                    CasualLeaves = table.Column<int>(type: "int", nullable: false),
                    Faculty = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    DutyShift = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    NoOfIncrements = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstimatedBudgetLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalculationType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimatedBudgetLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimatedBudgetLines_EstimatedBudgetMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "EstimatedBudgetMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstimatedBudgetLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GrnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaster_PurchaseOrderMaster_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrderMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrderMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "PurchaseOrderMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitionLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionLines_RequisitionMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "RequisitionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitionLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollItemEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayrollItemId = table.Column<int>(type: "int", nullable: false),
                    PayrollType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollItemEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollItemEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollItemEmployees_PayrollItems_PayrollItemId",
                        column: x => x.PayrollItemId,
                        principalTable: "PayrollItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollTransactionMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BPSAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BPSName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    AccountPayableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    PresentDays = table.Column<int>(type: "int", nullable: false),
                    LeaveDays = table.Column<int>(type: "int", nullable: false),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollTransactionMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_Level4_AccountPayableId",
                        column: x => x.AccountPayableId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNLines_GRNMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GRNLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollTransactionLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayrollItemId = table.Column<int>(type: "int", nullable: false),
                    PayrollType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollTransactionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionLines_PayrollItems_PayrollItemId",
                        column: x => x.PayrollItemId,
                        principalTable: "PayrollItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollTransactionLines_PayrollTransactionMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "PayrollTransactionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BusinessPartnerId",
                table: "Employees",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DesignationId",
                table: "Employees",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimatedBudgetLines_AccountId",
                table: "EstimatedBudgetLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimatedBudgetLines_MasterId",
                table: "EstimatedBudgetLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimatedBudgetMaster_BudgetId",
                table: "EstimatedBudgetMaster",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNLines_ItemId",
                table: "GRNLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNLines_MasterId",
                table: "GRNLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNLines_WarehouseId",
                table: "GRNLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaster_CampusId",
                table: "GRNMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaster_PurchaseOrderId",
                table: "GRNMaster",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaster_StatusId",
                table: "GRNMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaster_VendorId",
                table: "GRNMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollItemEmployees_EmployeeId",
                table: "PayrollItemEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollItemEmployees_PayrollItemId",
                table: "PayrollItemEmployees",
                column: "PayrollItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollItems_AccountId",
                table: "PayrollItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionLines_AccountId",
                table: "PayrollTransactionLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionLines_MasterId",
                table: "PayrollTransactionLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionLines_PayrollItemId",
                table: "PayrollTransactionLines",
                column: "PayrollItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_AccountPayableId",
                table: "PayrollTransactionMaster",
                column: "AccountPayableId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_DepartmentId",
                table: "PayrollTransactionMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_DesignationId",
                table: "PayrollTransactionMaster",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_EmployeeId",
                table: "PayrollTransactionMaster",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_StatusId",
                table: "PayrollTransactionMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_TransactionId",
                table: "PayrollTransactionMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_AccountId",
                table: "PurchaseOrderLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ItemId",
                table: "PurchaseOrderLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_MasterId",
                table: "PurchaseOrderLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_WarehouseId",
                table: "PurchaseOrderLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMaster_CampusId",
                table: "PurchaseOrderMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMaster_StatusId",
                table: "PurchaseOrderMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMaster_VendorId",
                table: "PurchaseOrderMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionLines_ItemId",
                table: "RequisitionLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionLines_MasterId",
                table: "RequisitionLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionLines_WarehouseId",
                table: "RequisitionLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_BusinessPartnerId",
                table: "RequisitionMaster",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_CampusId",
                table: "RequisitionMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_StatusId",
                table: "RequisitionMaster",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstimatedBudgetLines");

            migrationBuilder.DropTable(
                name: "GRNLines");

            migrationBuilder.DropTable(
                name: "PayrollItemEmployees");

            migrationBuilder.DropTable(
                name: "PayrollTransactionLines");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines");

            migrationBuilder.DropTable(
                name: "RequisitionLines");

            migrationBuilder.DropTable(
                name: "EstimatedBudgetMaster");

            migrationBuilder.DropTable(
                name: "GRNMaster");

            migrationBuilder.DropTable(
                name: "PayrollItems");

            migrationBuilder.DropTable(
                name: "PayrollTransactionMaster");

            migrationBuilder.DropTable(
                name: "RequisitionMaster");

            migrationBuilder.DropTable(
                name: "PurchaseOrderMaster");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "RecordLedger");

            migrationBuilder.DropColumn(
                name: "SRBTax",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OtherTax",
                table: "BillMaster");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "BillMaster");

            migrationBuilder.DropColumn(
                name: "AnyOtherTax",
                table: "BillLines");

            migrationBuilder.DropColumn(
                name: "BankAccountType",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "StoreManager",
                table: "Warehouses",
                newName: "Manager");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountReceivableId",
                table: "BusinessPartners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountPayableId",
                table: "BusinessPartners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
