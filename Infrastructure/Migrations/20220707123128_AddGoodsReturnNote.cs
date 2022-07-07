using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddGoodsReturnNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "IssuanceLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "GRNMaster",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IssuanceId",
                table: "GRNMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GRNLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IssuanceToGRNLineReconcile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IssuanceId = table.Column<int>(type: "int", nullable: false),
                    GRNId = table.Column<int>(type: "int", nullable: false),
                    IssuanceLineId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IssuanceToGRNLineReconcile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuanceToGRNLineReconcile_GRNLines_GRNLineId",
                        column: x => x.GRNLineId,
                        principalTable: "GRNLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToGRNLineReconcile_GRNMaster_GRNId",
                        column: x => x.GRNId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToGRNLineReconcile_IssuanceLines_IssuanceLineId",
                        column: x => x.IssuanceLineId,
                        principalTable: "IssuanceLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToGRNLineReconcile_IssuanceMaster_IssuanceId",
                        column: x => x.IssuanceId,
                        principalTable: "IssuanceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToGRNLineReconcile_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuanceToGRNLineReconcile_Warehouses_WarehouseId",
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
                name: "GRNToGoodsReturnNoteReconcile",
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
                    table.PrimaryKey("PK_GRNToGoodsReturnNoteReconcile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteLines_GoodsReturnNoteLineId",
                        column: x => x.GoodsReturnNoteLineId,
                        principalTable: "GoodsReturnNoteLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteMaster_GoodsReturnNoteId",
                        column: x => x.GoodsReturnNoteId,
                        principalTable: "GoodsReturnNoteMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteReconcile_GRNLines_GRNLineId",
                        column: x => x.GRNLineId,
                        principalTable: "GRNLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteReconcile_GRNMaster_GRNId",
                        column: x => x.GRNId,
                        principalTable: "GRNMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteReconcile_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNToGoodsReturnNoteReconcile_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaster_IssuanceId",
                table: "GRNMaster",
                column: "IssuanceId");

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
                name: "IX_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GoodsReturnNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GoodsReturnNoteLineId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GRNId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GRNLineId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GRNLineId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_ItemId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_WarehouseId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToGRNLineReconcile_GRNId",
                table: "IssuanceToGRNLineReconcile",
                column: "GRNId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToGRNLineReconcile_GRNLineId",
                table: "IssuanceToGRNLineReconcile",
                column: "GRNLineId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToGRNLineReconcile_IssuanceId",
                table: "IssuanceToGRNLineReconcile",
                column: "IssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToGRNLineReconcile_IssuanceLineId",
                table: "IssuanceToGRNLineReconcile",
                column: "IssuanceLineId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToGRNLineReconcile_ItemId",
                table: "IssuanceToGRNLineReconcile",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceToGRNLineReconcile_WarehouseId",
                table: "IssuanceToGRNLineReconcile",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNMaster_IssuanceMaster_IssuanceId",
                table: "GRNMaster",
                column: "IssuanceId",
                principalTable: "IssuanceMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNMaster_IssuanceMaster_IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropTable(
                name: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropTable(
                name: "IssuanceToGRNLineReconcile");

            migrationBuilder.DropTable(
                name: "GoodsReturnNoteLines");

            migrationBuilder.DropTable(
                name: "GoodsReturnNoteMaster");

            migrationBuilder.DropIndex(
                name: "IX_GRNMaster_IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "IssuanceLines");

            migrationBuilder.DropColumn(
                name: "IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GRNLines");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "GRNMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
