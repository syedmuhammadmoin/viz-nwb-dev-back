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
                name: "BudgetMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "Level1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                name: "TransactionReconciles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTransactionId = table.Column<int>(type: "int", nullable: false),
                    DocumentTransactionId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_TransactionReconciles_Transactions_DocumentTransactionId",
                        column: x => x.DocumentTransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionReconciles_Transactions_PaymentTransactionId",
                        column: x => x.PaymentTransactionId,
                        principalTable: "Transactions",
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
                    Manager = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    AccountReceivableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountPayableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentRegisterType = table.Column<int>(type: "int", nullable: false),
                    PaymentRegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    GrossPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    BankReconStatus = table.Column<int>(type: "int", nullable: true),
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
                table: "Organizations",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedBy", "CreatedDate", "Email", "Fax", "FiscalYear", "GSTRegistrationNo", "IncomeTaxId", "Industry", "IsDelete", "LegalStatus", "ModifiedBy", "ModifiedDate", "Name", "Phone", "StartDate", "State", "Website" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, null, null, false, null, null, null, "SBBU", null, null, null, null });

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
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

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
                name: "IX_TransactionReconciles_DocumentTransactionId",
                table: "TransactionReconciles",
                column: "DocumentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReconciles_PaymentTransactionId",
                table: "TransactionReconciles",
                column: "PaymentTransactionId");

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
                name: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "JournalEntryLines");

            migrationBuilder.DropTable(
                name: "RecordLedger");

            migrationBuilder.DropTable(
                name: "RoleClaims");

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
                name: "BudgetMaster");

            migrationBuilder.DropTable(
                name: "CreditNoteMaster");

            migrationBuilder.DropTable(
                name: "DebitNoteMaster");

            migrationBuilder.DropTable(
                name: "InvoiceMaster");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "JournalEntryMaster");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WorkFlowMaster");

            migrationBuilder.DropTable(
                name: "BankStmtMaster");

            migrationBuilder.DropTable(
                name: "BusinessPartners");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "WorkFlowStatus");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropTable(
                name: "Level4");

            migrationBuilder.DropTable(
                name: "Transactions");

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
