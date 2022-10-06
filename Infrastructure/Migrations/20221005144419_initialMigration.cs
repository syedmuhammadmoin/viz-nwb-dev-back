using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Level1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LegalStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncomeTaxId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GSTRegistrationNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FiscalYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocId = table.Column<int>(type: "int", nullable: false),
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
                name: "UnitOfMeasurement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Level2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Level1_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Level2_Level1_Level1_id",
                        column: x => x.Level1_id,
                        principalTable: "Level1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Campuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesTaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SRB = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowTransitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentStatusId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    NextStatusId = table.Column<int>(type: "int", nullable: false),
                    AllowedRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFlowTransitions_Roles_AllowedRoleId",
                        column: x => x.AllowedRoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTransitions_WorkFlowMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "WorkFlowMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkFlowTransitions_WorkFlowStatus_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTransitions_WorkFlowStatus_NextStatusId",
                        column: x => x.NextStatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Level3",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Level2_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Level3_Level2_Level2_id",
                        column: x => x.Level2_id,
                        principalTable: "Level2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BudgetMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreManager = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Level4",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    Level3_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level1_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Level4_Level1_Level1_id",
                        column: x => x.Level1_id,
                        principalTable: "Level1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Level4_Level3_Level3_id",
                        column: x => x.Level3_id,
                        principalTable: "Level3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankAccountType = table.Column<int>(type: "int", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClearingAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_BankAccounts_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "BudgetLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLines_BudgetMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "BudgetMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPartners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessPartnerType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IncomeTaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesTaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankAccountTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AccountReceivableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountPayableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartners_Level4_AccountPayableId",
                        column: x => x.AccountPayableId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessPartners_Level4_AccountReceivableId",
                        column: x => x.AccountReceivableId,
                        principalTable: "Level4",
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
                    OpeningBalanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ChAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_CashAccounts_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InventoryAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevenueAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Level4_CostAccountId",
                        column: x => x.CostAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Level4_InventoryAccountId",
                        column: x => x.InventoryAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Level4_RevenueAccountId",
                        column: x => x.RevenueAccountId,
                        principalTable: "Level4",
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
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    TaxType = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxes_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
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
                name: "CreditNoteMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NoteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    DocumentLedgerId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditNoteMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditNoteMaster_BusinessPartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditNoteMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditNoteMaster_Level4_ReceivableAccountId",
                        column: x => x.ReceivableAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditNoteMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditNoteMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DebitNoteMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NoteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    DocumentLedgerId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitNoteMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebitNoteMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNoteMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNoteMaster_Level4_PayableAccountId",
                        column: x => x.PayableAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNoteMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNoteMaster_WorkFlowStatus_StatusId",
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
                    EmployeeType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DateofJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateofRetirment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EarnedLeaves = table.Column<int>(type: "int", nullable: false),
                    CasualLeaves = table.Column<int>(type: "int", nullable: false),
                    Faculty = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    DutyShift = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    NoOfIncrements = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_Employees_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
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
                name: "InvoiceMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReceivableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceMaster_BusinessPartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceMaster_Level4_ReceivableAccountId",
                        column: x => x.ReceivableAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_JournalEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_JournalEntryMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "JournalEntryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: false),
                    PaymentFormType = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentRegisterType = table.Column<int>(type: "int", nullable: false),
                    PaymentRegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    GrossPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SRBTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeductionAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OtherDeductionAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NetPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    BankReconStatus = table.Column<int>(type: "int", nullable: true),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    DocumentLedgerId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Level4_OtherDeductionAccountId",
                        column: x => x.OtherDeductionAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Level4_PaymentRegisterId",
                        column: x => x.PaymentRegisterId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
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
                name: "RecordLedger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Level4_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sign = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReconStatus = table.Column<int>(type: "int", nullable: false),
                    IsReconcilable = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_RecordLedger_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLedger_Level4_Level4_id",
                        column: x => x.Level4_id,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLedger_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLedger_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasurementId = table.Column<int>(type: "int", nullable: false),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_UnitOfMeasurement_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurement",
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
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    AccountPayableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    PresentDays = table.Column<int>(type: "int", nullable: false),
                    LeaveDays = table.Column<int>(type: "int", nullable: false),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollTransactionMaster", x => x.Id);
                    table.UniqueConstraint("AK_PayrollTransactionMaster_Month_Year_EmployeeId", x => new { x.Month, x.Year, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_PayrollTransactionMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "RequisitionMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_RequisitionMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionMaster_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
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
                name: "TransactionReconciles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentLedgerId = table.Column<int>(type: "int", nullable: false),
                    DocumentLegderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionReconciles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionReconciles_RecordLedger_DocumentLegderId",
                        column: x => x.DocumentLegderId,
                        principalTable: "RecordLedger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionReconciles_RecordLedger_PaymentLedgerId",
                        column: x => x.PaymentLedgerId,
                        principalTable: "RecordLedger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditNoteLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_CreditNoteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_CreditNoteMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "CreditNoteMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditNoteLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DebitNoteLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_DebitNoteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebitNoteLines_DebitNoteMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "DebitNoteMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DebitNoteLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNoteLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebitNoteLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_InvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_InvoiceMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "InvoiceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
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
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
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
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "IssuanceMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    IssuanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RequisitionId = table.Column<int>(type: "int", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceMaster_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceMaster_RequisitionMaster_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "RequisitionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
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
                    Status = table.Column<int>(type: "int", nullable: false),
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
                name: "FileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocId = table.Column<int>(type: "int", nullable: false),
                    DocType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUpload_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Remarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocId = table.Column<int>(type: "int", nullable: false),
                    DocType = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    GRNId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillMaster_GRNMaster_GRNId",
                        column: x => x.GRNId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillMaster_Level4_PayableAccountId",
                        column: x => x.PayableAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReturnNoteMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TotalBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    GRNId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReturnNoteMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteMaster_GRNMaster_GRNId",
                        column: x => x.GRNId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteMaster_WorkFlowStatus_StatusId",
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
                    Status = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
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
                name: "IssuanceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceLines_IssuanceMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "IssuanceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuanceLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuanceReturnMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IssuanceReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    IssuanceId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceReturnMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnMaster_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnMaster_IssuanceMaster_IssuanceId",
                        column: x => x.IssuanceId,
                        principalTable: "IssuanceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnyOtherTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BillLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillLines_BillMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "BillMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillLines_Level4_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReturnNoteLines",
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
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReturnNoteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteLines_GoodsReturnNoteMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "GoodsReturnNoteMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodsReturnNoteLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POToGRNLineReconcile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    GRNId = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderLineId = table.Column<int>(type: "int", nullable: false),
                    GRNLineId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POToGRNLineReconcile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POToGRNLineReconcile_GRNLines_GRNLineId",
                        column: x => x.GRNLineId,
                        principalTable: "GRNLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POToGRNLineReconcile_GRNMaster_GRNId",
                        column: x => x.GRNId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POToGRNLineReconcile_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POToGRNLineReconcile_PurchaseOrderLines_PurchaseOrderLineId",
                        column: x => x.PurchaseOrderLineId,
                        principalTable: "PurchaseOrderLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POToGRNLineReconcile_PurchaseOrderMaster_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrderMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POToGRNLineReconcile_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionToIssuanceLineReconcile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RequisitionId = table.Column<int>(type: "int", nullable: false),
                    IssuanceId = table.Column<int>(type: "int", nullable: false),
                    RequisitionLineId = table.Column<int>(type: "int", nullable: false),
                    IssuanceLineId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionToIssuanceLineReconcile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitionToIssuanceLineReconcile_IssuanceLines_IssuanceLineId",
                        column: x => x.IssuanceLineId,
                        principalTable: "IssuanceLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionToIssuanceLineReconcile_IssuanceMaster_IssuanceId",
                        column: x => x.IssuanceId,
                        principalTable: "IssuanceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionToIssuanceLineReconcile_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionToIssuanceLineReconcile_RequisitionLines_RequisitionLineId",
                        column: x => x.RequisitionLineId,
                        principalTable: "RequisitionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionToIssuanceLineReconcile_RequisitionMaster_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "RequisitionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionToIssuanceLineReconcile_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuanceReturnLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceReturnLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnLines_IssuanceReturnMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "IssuanceReturnMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceReturnLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNToGoodsReturnNoteLineReconcile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    GoodsReturnNoteId = table.Column<int>(type: "int", nullable: false),
                    GRNId = table.Column<int>(type: "int", nullable: false),
                    GoodsReturnNoteLineId = table.Column<int>(type: "int", nullable: false),
                    GRNLineId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNToGoodsReturnNoteLineReconcile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteLines_GoodsReturnNoteLineId",
                        column: x => x.GoodsReturnNoteLineId,
                        principalTable: "GoodsReturnNoteLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteMaster_GoodsReturnNoteId",
                        column: x => x.GoodsReturnNoteId,
                        principalTable: "GoodsReturnNoteMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteLineReconcile_GRNLines_GRNLineId",
                        column: x => x.GRNLineId,
                        principalTable: "GRNLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteLineReconcile_GRNMaster_GRNId",
                        column: x => x.GRNId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteLineReconcile_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteLineReconcile_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssuanceToIssuanceReturnLineReconcile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IssuanceId = table.Column<int>(type: "int", nullable: false),
                    IssuanceReturnId = table.Column<int>(type: "int", nullable: false),
                    IssuanceLineId = table.Column<int>(type: "int", nullable: false),
                    IssuanceReturnLineId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuanceToIssuanceReturnLineReconcile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceToIssuanceReturnLineReconcile_IssuanceLines_IssuanceLineId",
                        column: x => x.IssuanceLineId,
                        principalTable: "IssuanceLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToIssuanceReturnLineReconcile_IssuanceMaster_IssuanceId",
                        column: x => x.IssuanceId,
                        principalTable: "IssuanceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToIssuanceReturnLineReconcile_IssuanceReturnLines_IssuanceReturnLineId",
                        column: x => x.IssuanceReturnLineId,
                        principalTable: "IssuanceReturnLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToIssuanceReturnLineReconcile_IssuanceReturnMaster_IssuanceReturnId",
                        column: x => x.IssuanceReturnId,
                        principalTable: "IssuanceReturnMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToIssuanceReturnLineReconcile_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToIssuanceReturnLineReconcile_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Level1",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("10000000-5566-7788-99aa-bbccddeeff00"), "F", null, null, false, null, null, "Assets" },
                    { new Guid("20000000-5566-7788-99aa-bbccddeeff00"), "G", null, null, false, null, null, "Liability" },
                    { new Guid("30000000-5566-7788-99aa-bbccddeeff00"), "P", null, null, false, null, null, "Accumulated Fund" },
                    { new Guid("40000000-5566-7788-99aa-bbccddeeff00"), "C", null, null, false, null, null, "Revenue" },
                    { new Guid("50000000-5566-7788-99aa-bbccddeeff00"), "A", null, null, false, null, null, "Expenses" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedBy", "CreatedDate", "Email", "Fax", "FiscalYear", "GSTRegistrationNo", "IncomeTaxId", "Industry", "IsDelete", "LegalStatus", "ModifiedBy", "ModifiedDate", "Name", "Phone", "StartDate", "State", "Website" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, null, null, false, null, null, null, "SBBU", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "AccountId", "CreatedBy", "CreatedDate", "IsDelete", "ModifiedBy", "ModifiedDate", "Name", "TaxType" },
                values: new object[,]
                {
                    { 1, null, null, null, false, null, null, "Sales Tax Asset", 0 },
                    { 2, null, null, null, false, null, null, "Sales Tax Liability", 1 },
                    { 3, null, null, null, false, null, null, "Income Tax Asset", 2 },
                    { 4, null, null, null, false, null, null, "Income Tax Liability", 3 },
                    { 5, null, null, null, false, null, null, "SRB Tax Asset", 4 },
                    { 6, null, null, null, false, null, null, "SRB Tax Liability", 5 }
                });

            migrationBuilder.InsertData(
                table: "WorkFlowStatus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDelete", "ModifiedBy", "ModifiedDate", "State", "Status", "Type" },
                values: new object[,]
                {
                    { 1, null, null, false, null, null, 0, "Draft", 1 },
                    { 2, null, null, false, null, null, 1, "Rejected", 2 },
                    { 3, null, null, false, null, null, 2, "Unpaid", 1 },
                    { 4, null, null, false, null, null, 3, "Partial Paid", 1 },
                    { 5, null, null, false, null, null, 4, "Paid", 1 },
                    { 6, null, null, false, null, null, 5, "Submitted", 2 },
                    { 7, null, null, false, null, null, 7, "Cancelled", 1 }
                });

            migrationBuilder.InsertData(
                table: "Level2",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level1_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("11000000-5566-7788-99aa-bbccddeeff00"), "F03", null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), null, null, "Non - Current Assets" },
                    { new Guid("12000000-5566-7788-99aa-bbccddeeff00"), "F02", null, null, false, new Guid("10000000-5566-7788-99aa-bbccddeeff00"), null, null, "Current Assets" },
                    { new Guid("21000000-5566-7788-99aa-bbccddeeff00"), "G02", null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), null, null, "Non - Current Liabilities" },
                    { new Guid("22000000-5566-7788-99aa-bbccddeeff00"), "G01", null, null, false, new Guid("20000000-5566-7788-99aa-bbccddeeff00"), null, null, "Current Liabilities" },
                    { new Guid("31000000-5566-7788-99aa-bbccddeeff00"), "P02", null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), null, null, "Grants" },
                    { new Guid("32000000-5566-7788-99aa-bbccddeeff00"), "P01", null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), null, null, "Surplus/(Deficit)" },
                    { new Guid("41000000-5566-7788-99aa-bbccddeeff00"), "C02", null, null, false, new Guid("40000000-5566-7788-99aa-bbccddeeff00"), null, null, "Onsite And Offsite Revenue" },
                    { new Guid("51000000-5566-7788-99aa-bbccddeeff00"), "A01", null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Employee Related Expenses" },
                    { new Guid("52000000-5566-7788-99aa-bbccddeeff00"), "A02", null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Project Pre-Investment Analysis" },
                    { new Guid("53000000-5566-7788-99aa-bbccddeeff00"), "A03", null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Operating Expenses" },
                    { new Guid("54000000-5566-7788-99aa-bbccddeeff00"), "A04", null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Employees Retirement Benefits" },
                    { new Guid("55000000-5566-7788-99aa-bbccddeeff00"), "A06", null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Transfers" }
                });

            migrationBuilder.InsertData(
                table: "Level3",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level2_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("11100000-5566-7788-99aa-bbccddeeff00"), "F031", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Property Plant And Equipment" },
                    { new Guid("11200000-5566-7788-99aa-bbccddeeff00"), "F032", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Capital Work-In-Progress" },
                    { new Guid("11300000-5566-7788-99aa-bbccddeeff00"), "F033", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Intangible Assets" },
                    { new Guid("11400000-5566-7788-99aa-bbccddeeff00"), "F034", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Intangible Asset Under Development" },
                    { new Guid("11500000-5566-7788-99aa-bbccddeeff00"), "F035", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Long Term Loan" },
                    { new Guid("11600000-5566-7788-99aa-bbccddeeff00"), "F036", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Investment Property" },
                    { new Guid("11700000-5566-7788-99aa-bbccddeeff00"), "F037", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Long Term Investment" },
                    { new Guid("11800000-5566-7788-99aa-bbccddeeff00"), "F038", null, null, false, new Guid("11000000-5566-7788-99aa-bbccddeeff00"), null, null, "Advance To Employees" },
                    { new Guid("12100000-5566-7788-99aa-bbccddeeff00"), "F021", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Other Receivables" },
                    { new Guid("12110000-5566-7788-99aa-bbccddeeff00"), "F0210", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Rent Receivable" },
                    { new Guid("12120000-5566-7788-99aa-bbccddeeff00"), "F0211", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Interest Receivable" },
                    { new Guid("12200000-5566-7788-99aa-bbccddeeff00"), "F022", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Receivable From Government Authorities" },
                    { new Guid("12300000-5566-7788-99aa-bbccddeeff00"), "F023", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Affiliated Colleges Fee Receivable" },
                    { new Guid("12400000-5566-7788-99aa-bbccddeeff00"), "F024", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Inventory" },
                    { new Guid("12500000-5566-7788-99aa-bbccddeeff00"), "F025", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Cash balances with Banks " },
                    { new Guid("12600000-5566-7788-99aa-bbccddeeff00"), "F026", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Petty Cash" },
                    { new Guid("12700000-5566-7788-99aa-bbccddeeff00"), "F027", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Advances, Prepayments & Deposits" },
                    { new Guid("12800000-5566-7788-99aa-bbccddeeff00"), "F028", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Short Term Investments" },
                    { new Guid("12900000-5566-7788-99aa-bbccddeeff00"), "F029", null, null, false, new Guid("12000000-5566-7788-99aa-bbccddeeff00"), null, null, "Receivable From Students" },
                    { new Guid("21100000-5566-7788-99aa-bbccddeeff00"), "G021", null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Long Term Borrowings" },
                    { new Guid("21200000-5566-7788-99aa-bbccddeeff00"), "G022", null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Post Retirement Benefit Plan" },
                    { new Guid("21300000-5566-7788-99aa-bbccddeeff00"), "G023", null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Long Term Compensated Absences" },
                    { new Guid("21400000-5566-7788-99aa-bbccddeeff00"), "G024", null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Security Deposit-Non Current" },
                    { new Guid("21500000-5566-7788-99aa-bbccddeeff00"), "G025", null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Deferred Capital Grant" },
                    { new Guid("21600000-5566-7788-99aa-bbccddeeff00"), "G026", null, null, false, new Guid("21000000-5566-7788-99aa-bbccddeeff00"), null, null, "Others" },
                    { new Guid("22100000-5566-7788-99aa-bbccddeeff00"), "G01", null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Accounts Payable" },
                    { new Guid("22200000-5566-7788-99aa-bbccddeeff00"), "G02", null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Short Term Borrowings" },
                    { new Guid("22300000-5566-7788-99aa-bbccddeeff00"), "G03", null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Other Liabilities" },
                    { new Guid("22400000-5566-7788-99aa-bbccddeeff00"), "G04", null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Security Deposit-Short Term" },
                    { new Guid("22500000-5566-7788-99aa-bbccddeeff00"), "G05", null, null, false, new Guid("22000000-5566-7788-99aa-bbccddeeff00"), null, null, "Financial Assistance/ Scholarships" },
                    { new Guid("31100000-5566-7788-99aa-bbccddeeff00"), "P021", null, null, false, new Guid("31000000-5566-7788-99aa-bbccddeeff00"), null, null, "Federal Govt Grant" },
                    { new Guid("31200000-5566-7788-99aa-bbccddeeff00"), "P022", null, null, false, new Guid("31000000-5566-7788-99aa-bbccddeeff00"), null, null, "Sindh Govt Grant" },
                    { new Guid("32100000-5566-7788-99aa-bbccddeeff00"), "P011", null, null, false, new Guid("32000000-5566-7788-99aa-bbccddeeff00"), null, null, "Surplus/(Deficit) Of Comprehensive Income" },
                    { new Guid("32200000-5566-7788-99aa-bbccddeeff00"), "P012", null, null, false, new Guid("32000000-5566-7788-99aa-bbccddeeff00"), null, null, "Retained Earning" },
                    { new Guid("41100000-5566-7788-99aa-bbccddeeff00"), "CO21", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Education General Fees " },
                    { new Guid("41200000-5566-7788-99aa-bbccddeeff00"), "CO22", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Hostel Fees / User Charges " },
                    { new Guid("41300000-5566-7788-99aa-bbccddeeff00"), "CO23", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Income From Endowments" },
                    { new Guid("41400000-5566-7788-99aa-bbccddeeff00"), "CO24", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Income From Services Rendered " },
                    { new Guid("41500000-5566-7788-99aa-bbccddeeff00"), "CO25", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Income From Intellectual Property " },
                    { new Guid("41600000-5566-7788-99aa-bbccddeeff00"), "CO26", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Others" },
                    { new Guid("41700000-5566-7788-99aa-bbccddeeff00"), "CO27", null, null, false, new Guid("41000000-5566-7788-99aa-bbccddeeff00"), null, null, "Grant Revenue" },
                    { new Guid("51100000-5566-7788-99aa-bbccddeeff00"), "A011", null, null, false, new Guid("51000000-5566-7788-99aa-bbccddeeff00"), null, null, "Pay" }
                });

            migrationBuilder.InsertData(
                table: "Level3",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level2_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("51200000-5566-7788-99aa-bbccddeeff00"), "A012", null, null, false, new Guid("51000000-5566-7788-99aa-bbccddeeff00"), null, null, "Allowances" },
                    { new Guid("52100000-5566-7788-99aa-bbccddeeff00"), "A021", null, null, false, new Guid("52000000-5566-7788-99aa-bbccddeeff00"), null, null, "Feasibility Studies" },
                    { new Guid("52200000-5566-7788-99aa-bbccddeeff00"), "A022", null, null, false, new Guid("52000000-5566-7788-99aa-bbccddeeff00"), null, null, "Research Survey & Exploratory Operations" },
                    { new Guid("53100000-5566-7788-99aa-bbccddeeff00"), "A031", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Fees" },
                    { new Guid("53110000-5566-7788-99aa-bbccddeeff00"), "A040", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Repair And Maintenance" },
                    { new Guid("53200000-5566-7788-99aa-bbccddeeff00"), "A032", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Communication Expense" },
                    { new Guid("53300000-5566-7788-99aa-bbccddeeff00"), "A033", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Utilities Expense" },
                    { new Guid("53400000-5566-7788-99aa-bbccddeeff00"), "A034", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Occupancy Cost" },
                    { new Guid("53500000-5566-7788-99aa-bbccddeeff00"), "A035", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Operating Leases" },
                    { new Guid("53600000-5566-7788-99aa-bbccddeeff00"), "A036", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Motor Vehicles" },
                    { new Guid("53700000-5566-7788-99aa-bbccddeeff00"), "A037", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Consultancy & Contractual Work" },
                    { new Guid("53800000-5566-7788-99aa-bbccddeeff00"), "A038", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Travel & Transportation" },
                    { new Guid("53900000-5566-7788-99aa-bbccddeeff00"), "A039", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "General " },
                    { new Guid("54100000-5566-7788-99aa-bbccddeeff00"), "A041", null, null, false, new Guid("54000000-5566-7788-99aa-bbccddeeff00"), null, null, "Pension" },
                    { new Guid("55100000-5566-7788-99aa-bbccddeeff00"), "A061", null, null, false, new Guid("55000000-5566-7788-99aa-bbccddeeff00"), null, null, "Scholarships" },
                    { new Guid("55200000-5566-7788-99aa-bbccddeeff00"), "A062", null, null, false, new Guid("55000000-5566-7788-99aa-bbccddeeff00"), null, null, "Technical Assistance" },
                    { new Guid("55300000-5566-7788-99aa-bbccddeeff00"), "A063", null, null, false, new Guid("55000000-5566-7788-99aa-bbccddeeff00"), null, null, "Entertainment & Gifts" },
                    { new Guid("55400000-5566-7788-99aa-bbccddeeff00"), "A064", null, null, false, new Guid("55000000-5566-7788-99aa-bbccddeeff00"), null, null, "Other Transfer Payments" }
                });

            migrationBuilder.InsertData(
                table: "Level4",
                columns: new[] { "Id", "AccountType", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level1_id", "Level3_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[] { new Guid("32110000-5566-7788-99aa-bbccddeeff00"), 0, "P01101", null, null, false, new Guid("30000000-5566-7788-99aa-bbccddeeff00"), new Guid("32100000-5566-7788-99aa-bbccddeeff00"), null, null, "Opening Balance equity" });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CampusId",
                table: "BankAccounts",
                column: "CampusId");

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
                name: "IX_BillLines_AccountId",
                table: "BillLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BillLines_ItemId",
                table: "BillLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BillLines_MasterId",
                table: "BillLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BillLines_WarehouseId",
                table: "BillLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_CampusId",
                table: "BillMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_GRNId",
                table: "BillMaster",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_PayableAccountId",
                table: "BillMaster",
                column: "PayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_StatusId",
                table: "BillMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_TransactionId",
                table: "BillMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMaster_VendorId",
                table: "BillMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_AccountId",
                table: "BudgetLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLines_MasterId",
                table: "BudgetLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetMaster_CampusId",
                table: "BudgetMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_AccountPayableId",
                table: "BusinessPartners",
                column: "AccountPayableId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_AccountReceivableId",
                table: "BusinessPartners",
                column: "AccountReceivableId");

            migrationBuilder.CreateIndex(
                name: "IX_Campuses_OrganizationId",
                table: "Campuses",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccounts_CampusId",
                table: "CashAccounts",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccounts_ChAccountId",
                table: "CashAccounts",
                column: "ChAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashAccounts_TransactionId",
                table: "CashAccounts",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CostAccountId",
                table: "Categories",
                column: "CostAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_InventoryAccountId",
                table: "Categories",
                column: "InventoryAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RevenueAccountId",
                table: "Categories",
                column: "RevenueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_AccountId",
                table: "CreditNoteLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_ItemId",
                table: "CreditNoteLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_MasterId",
                table: "CreditNoteLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteLines_WarehouseId",
                table: "CreditNoteLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_CampusId",
                table: "CreditNoteMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_CustomerId",
                table: "CreditNoteMaster",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_ReceivableAccountId",
                table: "CreditNoteMaster",
                column: "ReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_StatusId",
                table: "CreditNoteMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteMaster_TransactionId",
                table: "CreditNoteMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteLines_AccountId",
                table: "DebitNoteLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteLines_ItemId",
                table: "DebitNoteLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteLines_MasterId",
                table: "DebitNoteLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteLines_WarehouseId",
                table: "DebitNoteLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_CampusId",
                table: "DebitNoteMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_PayableAccountId",
                table: "DebitNoteMaster",
                column: "PayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_StatusId",
                table: "DebitNoteMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_TransactionId",
                table: "DebitNoteMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteMaster_VendorId",
                table: "DebitNoteMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BusinessPartnerId",
                table: "Employees",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CampusId",
                table: "Employees",
                column: "CampusId");

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
                name: "IX_FileUpload_UserId",
                table: "FileUpload",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteLines_ItemId",
                table: "GoodsReturnNoteLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteLines_MasterId",
                table: "GoodsReturnNoteLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteLines_WarehouseId",
                table: "GoodsReturnNoteLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteMaster_CampusId",
                table: "GoodsReturnNoteMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteMaster_GRNId",
                table: "GoodsReturnNoteMaster",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteMaster_StatusId",
                table: "GoodsReturnNoteMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReturnNoteMaster_VendorId",
                table: "GoodsReturnNoteMaster",
                column: "VendorId");

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
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GoodsReturnNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GoodsReturnNoteLineId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GRNId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GRNLineId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GRNLineId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_ItemId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_WarehouseId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_AccountId",
                table: "InvoiceLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ItemId",
                table: "InvoiceLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_MasterId",
                table: "InvoiceLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_WarehouseId",
                table: "InvoiceLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_CampusId",
                table: "InvoiceMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_CustomerId",
                table: "InvoiceMaster",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_ReceivableAccountId",
                table: "InvoiceMaster",
                column: "ReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_StatusId",
                table: "InvoiceMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceMaster_TransactionId",
                table: "InvoiceMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceLines_ItemId",
                table: "IssuanceLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceLines_MasterId",
                table: "IssuanceLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceLines_WarehouseId",
                table: "IssuanceLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceMaster_CampusId",
                table: "IssuanceMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceMaster_EmployeeId",
                table: "IssuanceMaster",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceMaster_RequisitionId",
                table: "IssuanceMaster",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceMaster_StatusId",
                table: "IssuanceMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnLines_ItemId",
                table: "IssuanceReturnLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnLines_MasterId",
                table: "IssuanceReturnLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnLines_WarehouseId",
                table: "IssuanceReturnLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnMaster_CampusId",
                table: "IssuanceReturnMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnMaster_EmployeeId",
                table: "IssuanceReturnMaster",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnMaster_IssuanceId",
                table: "IssuanceReturnMaster",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceReturnMaster_StatusId",
                table: "IssuanceReturnMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToIssuanceReturnLineReconcile_IssuanceId",
                table: "IssuanceToIssuanceReturnLineReconcile",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToIssuanceReturnLineReconcile_IssuanceLineId",
                table: "IssuanceToIssuanceReturnLineReconcile",
                column: "IssuanceLineId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToIssuanceReturnLineReconcile_IssuanceReturnId",
                table: "IssuanceToIssuanceReturnLineReconcile",
                column: "IssuanceReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToIssuanceReturnLineReconcile_IssuanceReturnLineId",
                table: "IssuanceToIssuanceReturnLineReconcile",
                column: "IssuanceReturnLineId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToIssuanceReturnLineReconcile_ItemId",
                table: "IssuanceToIssuanceReturnLineReconcile",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToIssuanceReturnLineReconcile_WarehouseId",
                table: "IssuanceToIssuanceReturnLineReconcile",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_AccountId",
                table: "JournalEntryLines",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_BusinessPartnerId",
                table: "JournalEntryLines",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_MasterId",
                table: "JournalEntryLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_WarehouseId",
                table: "JournalEntryLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryMaster_CampusId",
                table: "JournalEntryMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryMaster_StatusId",
                table: "JournalEntryMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryMaster_TransactionId",
                table: "JournalEntryMaster",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Level2_Level1_id",
                table: "Level2",
                column: "Level1_id");

            migrationBuilder.CreateIndex(
                name: "IX_Level3_Level2_id",
                table: "Level3",
                column: "Level2_id");

            migrationBuilder.CreateIndex(
                name: "IX_Level4_Code",
                table: "Level4",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Level4_Level1_id",
                table: "Level4",
                column: "Level1_id");

            migrationBuilder.CreateIndex(
                name: "IX_Level4_Level3_id",
                table: "Level4",
                column: "Level3_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AccountId",
                table: "Payments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BusinessPartnerId",
                table: "Payments",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CampusId",
                table: "Payments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OtherDeductionAccountId",
                table: "Payments",
                column: "OtherDeductionAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentRegisterId",
                table: "Payments",
                column: "PaymentRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StatusId",
                table: "Payments",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId");

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
                name: "IX_PayrollTransactionMaster_CampusId",
                table: "PayrollTransactionMaster",
                column: "CampusId");

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
                name: "IX_POToGRNLineReconcile_GRNId",
                table: "POToGRNLineReconcile",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_POToGRNLineReconcile_GRNLineId",
                table: "POToGRNLineReconcile",
                column: "GRNLineId");

            migrationBuilder.CreateIndex(
                name: "IX_POToGRNLineReconcile_ItemId",
                table: "POToGRNLineReconcile",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_POToGRNLineReconcile_PurchaseOrderId",
                table: "POToGRNLineReconcile",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_POToGRNLineReconcile_PurchaseOrderLineId",
                table: "POToGRNLineReconcile",
                column: "PurchaseOrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_POToGRNLineReconcile_WarehouseId",
                table: "POToGRNLineReconcile",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitOfMeasurementId",
                table: "Products",
                column: "UnitOfMeasurementId");

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
                name: "IX_RecordLedger_BusinessPartnerId",
                table: "RecordLedger",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_CampusId",
                table: "RecordLedger",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_Level4_id",
                table: "RecordLedger",
                column: "Level4_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_TransactionId",
                table: "RecordLedger",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_WarehouseId",
                table: "RecordLedger",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_UserId",
                table: "Remarks",
                column: "UserId");

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
                name: "IX_RequisitionMaster_CampusId",
                table: "RequisitionMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_EmployeeId",
                table: "RequisitionMaster",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_StatusId",
                table: "RequisitionMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionToIssuanceLineReconcile_IssuanceId",
                table: "RequisitionToIssuanceLineReconcile",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionToIssuanceLineReconcile_IssuanceLineId",
                table: "RequisitionToIssuanceLineReconcile",
                column: "IssuanceLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionToIssuanceLineReconcile_ItemId",
                table: "RequisitionToIssuanceLineReconcile",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionToIssuanceLineReconcile_RequisitionId",
                table: "RequisitionToIssuanceLineReconcile",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionToIssuanceLineReconcile_RequisitionLineId",
                table: "RequisitionToIssuanceLineReconcile",
                column: "RequisitionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionToIssuanceLineReconcile_WarehouseId",
                table: "RequisitionToIssuanceLineReconcile",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ItemId",
                table: "Stock",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_WarehouseId",
                table: "Stock",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_AccountId",
                table: "Taxes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReconciles_DocumentLegderId",
                table: "TransactionReconciles",
                column: "DocumentLegderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReconciles_PaymentLedgerId",
                table: "TransactionReconciles",
                column: "PaymentLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CampusId",
                table: "Warehouses",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTransitions_AllowedRoleId",
                table: "WorkFlowTransitions",
                column: "AllowedRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTransitions_CurrentStatusId",
                table: "WorkFlowTransitions",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTransitions_MasterId",
                table: "WorkFlowTransitions",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTransitions_NextStatusId",
                table: "WorkFlowTransitions",
                column: "NextStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankReconciliations");

            migrationBuilder.DropTable(
                name: "BillLines");

            migrationBuilder.DropTable(
                name: "BudgetLines");

            migrationBuilder.DropTable(
                name: "CashAccounts");

            migrationBuilder.DropTable(
                name: "CreditNoteLines");

            migrationBuilder.DropTable(
                name: "DebitNoteLines");

            migrationBuilder.DropTable(
                name: "EstimatedBudgetLines");

            migrationBuilder.DropTable(
                name: "FileUpload");

            migrationBuilder.DropTable(
                name: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropTable(
                name: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "IssuanceToIssuanceReturnLineReconcile");

            migrationBuilder.DropTable(
                name: "JournalEntryLines");

            migrationBuilder.DropTable(
                name: "PayrollItemEmployees");

            migrationBuilder.DropTable(
                name: "PayrollTransactionLines");

            migrationBuilder.DropTable(
                name: "POToGRNLineReconcile");

            migrationBuilder.DropTable(
                name: "Remarks");

            migrationBuilder.DropTable(
                name: "RequisitionToIssuanceLineReconcile");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "TransactionReconciles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WorkFlowTransitions");

            migrationBuilder.DropTable(
                name: "BankStmtLines");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "BillMaster");

            migrationBuilder.DropTable(
                name: "CreditNoteMaster");

            migrationBuilder.DropTable(
                name: "DebitNoteMaster");

            migrationBuilder.DropTable(
                name: "EstimatedBudgetMaster");

            migrationBuilder.DropTable(
                name: "GoodsReturnNoteLines");

            migrationBuilder.DropTable(
                name: "InvoiceMaster");

            migrationBuilder.DropTable(
                name: "IssuanceReturnLines");

            migrationBuilder.DropTable(
                name: "JournalEntryMaster");

            migrationBuilder.DropTable(
                name: "PayrollItems");

            migrationBuilder.DropTable(
                name: "PayrollTransactionMaster");

            migrationBuilder.DropTable(
                name: "GRNLines");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines");

            migrationBuilder.DropTable(
                name: "IssuanceLines");

            migrationBuilder.DropTable(
                name: "RequisitionLines");

            migrationBuilder.DropTable(
                name: "RecordLedger");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WorkFlowMaster");

            migrationBuilder.DropTable(
                name: "BankStmtMaster");

            migrationBuilder.DropTable(
                name: "BudgetMaster");

            migrationBuilder.DropTable(
                name: "GoodsReturnNoteMaster");

            migrationBuilder.DropTable(
                name: "IssuanceReturnMaster");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "GRNMaster");

            migrationBuilder.DropTable(
                name: "IssuanceMaster");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurement");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "PurchaseOrderMaster");

            migrationBuilder.DropTable(
                name: "RequisitionMaster");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "WorkFlowStatus");

            migrationBuilder.DropTable(
                name: "BusinessPartners");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropTable(
                name: "Level4");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Level3");

            migrationBuilder.DropTable(
                name: "Level2");

            migrationBuilder.DropTable(
                name: "Level1");
        }
    }
}
