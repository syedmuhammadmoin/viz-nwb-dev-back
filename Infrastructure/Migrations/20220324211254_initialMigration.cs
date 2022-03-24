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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Level4");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WorkFlowStatus");

            migrationBuilder.DropTable(
                name: "Level3");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Level2");

            migrationBuilder.DropTable(
                name: "Level1");
        }
    }
}
