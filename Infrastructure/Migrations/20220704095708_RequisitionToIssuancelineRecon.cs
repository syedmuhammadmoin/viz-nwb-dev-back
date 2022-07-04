using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RequisitionToIssuancelineRecon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "RequisitionLines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RequisitionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequisitionId",
                table: "IssuanceMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "IssuanceLines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_IssuanceMaster_RequisitionId",
                table: "IssuanceMaster",
                column: "RequisitionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_IssuanceMaster_RequisitionMaster_RequisitionId",
                table: "IssuanceMaster",
                column: "RequisitionId",
                principalTable: "RequisitionMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuanceMaster_RequisitionMaster_RequisitionId",
                table: "IssuanceMaster");

            migrationBuilder.DropTable(
                name: "RequisitionToIssuanceLineReconcile");

            migrationBuilder.DropIndex(
                name: "IX_IssuanceMaster_RequisitionId",
                table: "IssuanceMaster");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RequisitionLines");

            migrationBuilder.DropColumn(
                name: "RequisitionId",
                table: "IssuanceMaster");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "RequisitionLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "IssuanceLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
