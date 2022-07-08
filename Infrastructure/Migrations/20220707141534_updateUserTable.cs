using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class updateUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteLines_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteMaster_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GRNLines_GRNLineId",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GRNMaster_GRNId",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_Products_ItemId",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_Warehouses_WarehouseId",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GRNToGoodsReturnNoteReconcile",
                table: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "GRNToGoodsReturnNoteReconcile",
                newName: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_WarehouseId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                newName: "IX_GRNToGoodsReturnNoteLineReconcile_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_ItemId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                newName: "IX_GRNToGoodsReturnNoteLineReconcile_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GRNLineId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                newName: "IX_GRNToGoodsReturnNoteLineReconcile_GRNLineId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GRNId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                newName: "IX_GRNToGoodsReturnNoteLineReconcile_GRNId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                newName: "IX_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteLineId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                newName: "IX_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GRNToGoodsReturnNoteLineReconcile",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteLines_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GoodsReturnNoteLineId",
                principalTable: "GoodsReturnNoteLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteMaster_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GoodsReturnNoteId",
                principalTable: "GoodsReturnNoteMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GRNLines_GRNLineId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GRNLineId",
                principalTable: "GRNLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GRNMaster_GRNId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "GRNId",
                principalTable: "GRNMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_Products_ItemId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "ItemId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_Warehouses_WarehouseId",
                table: "GRNToGoodsReturnNoteLineReconcile",
                column: "WarehouseId",
                principalTable: "Warehouses",
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
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteLines_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteMaster_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GRNLines_GRNLineId",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_GRNMaster_GRNId",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_Products_ItemId",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_GRNToGoodsReturnNoteLineReconcile_Warehouses_WarehouseId",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GRNToGoodsReturnNoteLineReconcile",
                table: "GRNToGoodsReturnNoteLineReconcile");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "GRNToGoodsReturnNoteLineReconcile",
                newName: "GRNToGoodsReturnNoteReconcile");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_WarehouseId",
                table: "GRNToGoodsReturnNoteReconcile",
                newName: "IX_GRNToGoodsReturnNoteReconcile_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_ItemId",
                table: "GRNToGoodsReturnNoteReconcile",
                newName: "IX_GRNToGoodsReturnNoteReconcile_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GRNLineId",
                table: "GRNToGoodsReturnNoteReconcile",
                newName: "IX_GRNToGoodsReturnNoteReconcile_GRNLineId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GRNId",
                table: "GRNToGoodsReturnNoteReconcile",
                newName: "IX_GRNToGoodsReturnNoteReconcile_GRNId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteReconcile",
                newName: "IX_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteLineId");

            migrationBuilder.RenameIndex(
                name: "IX_GRNToGoodsReturnNoteLineReconcile_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteReconcile",
                newName: "IX_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_GRNToGoodsReturnNoteReconcile",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteLines_GoodsReturnNoteLineId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GoodsReturnNoteLineId",
                principalTable: "GoodsReturnNoteLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GoodsReturnNoteMaster_GoodsReturnNoteId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GoodsReturnNoteId",
                principalTable: "GoodsReturnNoteMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GRNLines_GRNLineId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GRNLineId",
                principalTable: "GRNLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_GRNMaster_GRNId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "GRNId",
                principalTable: "GRNMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_Products_ItemId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "ItemId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GRNToGoodsReturnNoteReconcile_Warehouses_WarehouseId",
                table: "GRNToGoodsReturnNoteReconcile",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
