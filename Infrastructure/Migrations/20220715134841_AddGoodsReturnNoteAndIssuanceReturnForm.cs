using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddGoodsReturnNoteAndIssuanceReturnForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionMaster_BusinessPartners_BusinessPartnerId",
                table: "RequisitionMaster");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "BusinessPartnerId",
                table: "RequisitionMaster",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitionMaster_BusinessPartnerId",
                table: "RequisitionMaster",
                newName: "IX_RequisitionMaster_EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "RequisitionLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GRNMaster_IssuanceMaster_IssuanceId",
                table: "GRNMaster",
                column: "IssuanceId",
                principalTable: "IssuanceMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionMaster_Employees_EmployeeId",
                table: "RequisitionMaster",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNMaster_IssuanceMaster_IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionMaster_Employees_EmployeeId",
                table: "RequisitionMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropTable(
                name: "IssuanceToIssuanceReturnLineReconcile");

            migrationBuilder.DropTable(
                name: "GoodsReturnNoteLines");

            migrationBuilder.DropTable(
                name: "IssuanceReturnLines");

            migrationBuilder.DropTable(
                name: "GoodsReturnNoteMaster");

            migrationBuilder.DropTable(
                name: "IssuanceReturnMaster");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_GRNMaster_IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "IssuanceLines");

            migrationBuilder.DropColumn(
                name: "IssuanceId",
                table: "GRNMaster");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GRNLines");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "RequisitionMaster",
                newName: "BusinessPartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitionMaster_EmployeeId",
                table: "RequisitionMaster",
                newName: "IX_RequisitionMaster_BusinessPartnerId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "RequisitionLines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderId",
                table: "GRNMaster",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionMaster_BusinessPartners_BusinessPartnerId",
                table: "RequisitionMaster",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
