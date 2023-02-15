using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddDepreciationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepreciationId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixedAsset",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Depreciations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UseFullLife = table.Column<int>(type: "int", nullable: false),
                    AssetAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepreciationExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelType = table.Column<int>(type: "int", nullable: false),
                    DecliningRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depreciations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depreciations_Level4_AccumulatedDepreciationId",
                        column: x => x.AccumulatedDepreciationId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Depreciations_Level4_AssetAccountId",
                        column: x => x.AssetAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Depreciations_Level4_DepreciationExpenseId",
                        column: x => x.DepreciationExpenseId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CWIPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CwipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfAcquisition = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CWIPAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    CostOfAsset = table.Column<int>(type: "int", nullable: false),
                    AssetAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalvageValue = table.Column<int>(type: "int", nullable: true),
                    DepreciationApplicability = table.Column<bool>(type: "bit", nullable: false),
                    DepreciationId = table.Column<int>(type: "int", nullable: true),
                    ModelType = table.Column<int>(type: "int", nullable: false),
                    DepreciationExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UseFullLife = table.Column<int>(type: "int", nullable: true),
                    Quantinty = table.Column<int>(type: "int", nullable: false),
                    DecLiningRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ProrataBasis = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CWIPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CWIPs_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_Depreciations_DepreciationId",
                        column: x => x.DepreciationId,
                        principalTable: "Depreciations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_Level4_AccumulatedDepreciationId",
                        column: x => x.AccumulatedDepreciationId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_Level4_AssetAccountId",
                        column: x => x.AssetAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_Level4_CWIPAccountId",
                        column: x => x.CWIPAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_Level4_DepreciationExpenseId",
                        column: x => x.DepreciationExpenseId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CWIPs_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixedAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateofAcquisition = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    SalvageValue = table.Column<int>(type: "int", nullable: false),
                    DepreciationApplicability = table.Column<bool>(type: "bit", nullable: false),
                    DepreciationId = table.Column<int>(type: "int", nullable: true),
                    ModelType = table.Column<int>(type: "int", nullable: false),
                    AssetAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepreciationExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UseFullLife = table.Column<int>(type: "int", nullable: true),
                    DecLiningRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProrataBasis = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Depreciations_DepreciationId",
                        column: x => x.DepreciationId,
                        principalTable: "Depreciations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Level4_AccumulatedDepreciationId",
                        column: x => x.AccumulatedDepreciationId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Level4_AssetAccountId",
                        column: x => x.AssetAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Level4_DepreciationExpenseId",
                        column: x => x.DepreciationExpenseId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedAssets_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Disposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalvageValue = table.Column<int>(type: "int", nullable: false),
                    UseFullLife = table.Column<int>(type: "int", nullable: false),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisposalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disposals_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disposals_FixedAssets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "FixedAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disposals_Level4_AccumulatedDepreciationId",
                        column: x => x.AccumulatedDepreciationId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disposals_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disposals_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DepreciationId",
                table: "Categories",
                column: "DepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_AccumulatedDepreciationId",
                table: "CWIPs",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_AssetAccountId",
                table: "CWIPs",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_CampusId",
                table: "CWIPs",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_CWIPAccountId",
                table: "CWIPs",
                column: "CWIPAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_DepreciationExpenseId",
                table: "CWIPs",
                column: "DepreciationExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_DepreciationId",
                table: "CWIPs",
                column: "DepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_StatusId",
                table: "CWIPs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_WarehouseId",
                table: "CWIPs",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Depreciations_AccumulatedDepreciationId",
                table: "Depreciations",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_Depreciations_AssetAccountId",
                table: "Depreciations",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Depreciations_DepreciationExpenseId",
                table: "Depreciations",
                column: "DepreciationExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_AccumulatedDepreciationId",
                table: "Disposals",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_AssetId",
                table: "Disposals",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_CategoryId",
                table: "Disposals",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_StatusId",
                table: "Disposals",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_WarehouseId",
                table: "Disposals",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_AccumulatedDepreciationId",
                table: "FixedAssets",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_AssetAccountId",
                table: "FixedAssets",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_CampusId",
                table: "FixedAssets",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_CategoryId",
                table: "FixedAssets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_DepreciationExpenseId",
                table: "FixedAssets",
                column: "DepreciationExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_DepreciationId",
                table: "FixedAssets",
                column: "DepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_StatusId",
                table: "FixedAssets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_WarehouseId",
                table: "FixedAssets",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Depreciations_DepreciationId",
                table: "Categories",
                column: "DepreciationId",
                principalTable: "Depreciations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Depreciations_DepreciationId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "CWIPs");

            migrationBuilder.DropTable(
                name: "Disposals");

            migrationBuilder.DropTable(
                name: "FixedAssets");

            migrationBuilder.DropTable(
                name: "Depreciations");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DepreciationId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DepreciationId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsFixedAsset",
                table: "Categories");
        }
    }
}
