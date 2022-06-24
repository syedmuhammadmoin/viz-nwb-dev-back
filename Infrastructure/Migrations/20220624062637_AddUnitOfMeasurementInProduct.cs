using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddUnitOfMeasurementInProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasurementId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitOfMeasurementId",
                table: "Products",
                column: "UnitOfMeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitOfMeasurement_UnitOfMeasurementId",
                table: "Products",
                column: "UnitOfMeasurementId",
                principalTable: "UnitOfMeasurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitOfMeasurement_UnitOfMeasurementId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitOfMeasurementId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasurementId",
                table: "Products");
        }
    }
}
