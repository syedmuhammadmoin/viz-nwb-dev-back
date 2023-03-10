using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddBudgetAppropiationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetReappropriationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    BudgetReappropriationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetReappropriationMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetReappropriationMaster_BudgetMaster_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "BudgetMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetReappropriationMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepreciationAdjustmentMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfDepreciationAdjustment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_DepreciationAdjustmentMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationAdjustmentMaster_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepreciationAdjustmentMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetReappropriationLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level4Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdditionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeletionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetReappropriationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetReappropriationLines_BudgetReappropriationMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "BudgetReappropriationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetReappropriationLines_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetReappropriationLines_Level4_Level4Id",
                        column: x => x.Level4Id,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepreciationAdjustmentLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixedAssetId = table.Column<int>(type: "int", nullable: false),
                    Level4Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepreciationAdjustmentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationAdjustmentLines_DepreciationAdjustmentMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "DepreciationAdjustmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepreciationAdjustmentLines_FixedAssets_FixedAssetId",
                        column: x => x.FixedAssetId,
                        principalTable: "FixedAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepreciationAdjustmentLines_Level4_Level4Id",
                        column: x => x.Level4Id,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetReappropriationLines_CampusId",
                table: "BudgetReappropriationLines",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetReappropriationLines_Level4Id",
                table: "BudgetReappropriationLines",
                column: "Level4Id");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetReappropriationLines_MasterId",
                table: "BudgetReappropriationLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetReappropriationMaster_BudgetId",
                table: "BudgetReappropriationMaster",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetReappropriationMaster_StatusId",
                table: "BudgetReappropriationMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationAdjustmentLines_FixedAssetId",
                table: "DepreciationAdjustmentLines",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationAdjustmentLines_Level4Id",
                table: "DepreciationAdjustmentLines",
                column: "Level4Id");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationAdjustmentLines_MasterId",
                table: "DepreciationAdjustmentLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationAdjustmentMaster_StatusId",
                table: "DepreciationAdjustmentMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationAdjustmentMaster_TransactionId",
                table: "DepreciationAdjustmentMaster",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetReappropriationLines");

            migrationBuilder.DropTable(
                name: "DepreciationAdjustmentLines");

            migrationBuilder.DropTable(
                name: "BudgetReappropriationMaster");

            migrationBuilder.DropTable(
                name: "DepreciationAdjustmentMaster");
        }
    }
}
