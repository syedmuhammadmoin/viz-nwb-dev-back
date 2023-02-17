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
                name: "FixedAssetId",
                table: "RequisitionLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FixedAssetId",
                table: "IssuanceLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepreciationModelId",
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
                name: "DepreciationModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_DepreciationModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationModels_Level4_AccumulatedDepreciationId",
                        column: x => x.AccumulatedDepreciationId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepreciationModels_Level4_AssetAccountId",
                        column: x => x.AssetAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepreciationModels_Level4_DepreciationExpenseId",
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
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CWIPAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    SalvageValue = table.Column<int>(type: "int", nullable: true),
                    DepreciationApplicability = table.Column<bool>(type: "bit", nullable: false),
                    DepreciationModelId = table.Column<int>(type: "int", nullable: true),
                    UseFullLife = table.Column<int>(type: "int", nullable: true),
                    AssetAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepreciationExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModelType = table.Column<int>(type: "int", nullable: false),
                    DecLiningRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProrataBasis = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_CWIPs_DepreciationModels_DepreciationModelId",
                        column: x => x.DepreciationModelId,
                        principalTable: "DepreciationModels",
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
                        name: "FK_CWIPs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    SalvageValue = table.Column<int>(type: "int", nullable: false),
                    DepreciationApplicability = table.Column<bool>(type: "bit", nullable: false),
                    DepreciationModelId = table.Column<int>(type: "int", nullable: true),
                    UseFullLife = table.Column<int>(type: "int", nullable: true),
                    AssetAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepreciationExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModelType = table.Column<int>(type: "int", nullable: false),
                    DecLiningRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProrataBasis = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    IsHeldforSaleOrDisposal = table.Column<bool>(type: "bit", nullable: false),
                    IsIssued = table.Column<bool>(type: "bit", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    IsDisposed = table.Column<bool>(type: "bit", nullable: false),
                    DocId = table.Column<int>(type: "int", nullable: false),
                    Doctype = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_FixedAssets_DepreciationModels_DepreciationModelId",
                        column: x => x.DepreciationModelId,
                        principalTable: "DepreciationModels",
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
                        name: "FK_FixedAssets_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                    FixedAssetId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalvageValue = table.Column<int>(type: "int", nullable: false),
                    UseFullLife = table.Column<int>(type: "int", nullable: false),
                    AccumulatedDepreciationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                        name: "FK_Disposals_FixedAssets_FixedAssetId",
                        column: x => x.FixedAssetId,
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
                        name: "FK_Disposals_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                name: "IX_RequisitionLines_FixedAssetId",
                table: "RequisitionLines",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceLines_FixedAssetId",
                table: "IssuanceLines",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DepreciationModelId",
                table: "Categories",
                column: "DepreciationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_AccumulatedDepreciationId",
                table: "CWIPs",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_AssetAccountId",
                table: "CWIPs",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_CWIPAccountId",
                table: "CWIPs",
                column: "CWIPAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_DepreciationExpenseId",
                table: "CWIPs",
                column: "DepreciationExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_DepreciationModelId",
                table: "CWIPs",
                column: "DepreciationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_ProductId",
                table: "CWIPs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_StatusId",
                table: "CWIPs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_WarehouseId",
                table: "CWIPs",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationModels_AccumulatedDepreciationId",
                table: "DepreciationModels",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationModels_AssetAccountId",
                table: "DepreciationModels",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationModels_DepreciationExpenseId",
                table: "DepreciationModels",
                column: "DepreciationExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_AccumulatedDepreciationId",
                table: "Disposals",
                column: "AccumulatedDepreciationId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_FixedAssetId",
                table: "Disposals",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_ProductId",
                table: "Disposals",
                column: "ProductId");

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
                name: "IX_FixedAssets_DepreciationExpenseId",
                table: "FixedAssets",
                column: "DepreciationExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_DepreciationModelId",
                table: "FixedAssets",
                column: "DepreciationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ProductId",
                table: "FixedAssets",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_StatusId",
                table: "FixedAssets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_WarehouseId",
                table: "FixedAssets",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_DepreciationModels_DepreciationModelId",
                table: "Categories",
                column: "DepreciationModelId",
                principalTable: "DepreciationModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuanceLines_FixedAssets_FixedAssetId",
                table: "IssuanceLines",
                column: "FixedAssetId",
                principalTable: "FixedAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionLines_FixedAssets_FixedAssetId",
                table: "RequisitionLines",
                column: "FixedAssetId",
                principalTable: "FixedAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DepreciationModels_DepreciationModelId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuanceLines_FixedAssets_FixedAssetId",
                table: "IssuanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionLines_FixedAssets_FixedAssetId",
                table: "RequisitionLines");

            migrationBuilder.DropTable(
                name: "CWIPs");

            migrationBuilder.DropTable(
                name: "Disposals");

            migrationBuilder.DropTable(
                name: "FixedAssets");

            migrationBuilder.DropTable(
                name: "DepreciationModels");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionLines_FixedAssetId",
                table: "RequisitionLines");

            migrationBuilder.DropIndex(
                name: "IX_IssuanceLines_FixedAssetId",
                table: "IssuanceLines");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DepreciationModelId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FixedAssetId",
                table: "RequisitionLines");

            migrationBuilder.DropColumn(
                name: "FixedAssetId",
                table: "IssuanceLines");

            migrationBuilder.DropColumn(
                name: "DepreciationModelId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsFixedAsset",
                table: "Categories");
        }
    }
}
