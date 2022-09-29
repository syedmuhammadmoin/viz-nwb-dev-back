using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdatedCOAOfRepairAndMaintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56100000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56200000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56300000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56400000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56500000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56600000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56700000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56800000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("56900000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.DeleteData(
                table: "Level2",
                keyColumn: "Id",
                keyValue: new Guid("56000000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.InsertData(
                table: "Level3",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level2_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[] { new Guid("53110000-5566-7788-99aa-bbccddeeff00"), "A040", null, null, false, new Guid("53000000-5566-7788-99aa-bbccddeeff00"), null, null, "Repair And Maintenance" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Level3",
                keyColumn: "Id",
                keyValue: new Guid("53110000-5566-7788-99aa-bbccddeeff00"));

            migrationBuilder.InsertData(
                table: "Level2",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level1_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[] { new Guid("56000000-5566-7788-99aa-bbccddeeff00"), "A13", null, null, false, new Guid("50000000-5566-7788-99aa-bbccddeeff00"), null, null, "Repair And Maintenance" });

            migrationBuilder.InsertData(
                table: "Level3",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDelete", "Level2_id", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("56100000-5566-7788-99aa-bbccddeeff00"), "A130", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Transport" },
                    { new Guid("56200000-5566-7788-99aa-bbccddeeff00"), "A131", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Machinary & Equipment" },
                    { new Guid("56300000-5566-7788-99aa-bbccddeeff00"), "A132", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Furniture & Fixture" },
                    { new Guid("56400000-5566-7788-99aa-bbccddeeff00"), "A133", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Building & Structure" },
                    { new Guid("56500000-5566-7788-99aa-bbccddeeff00"), "A137", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Computer Equipments" },
                    { new Guid("56600000-5566-7788-99aa-bbccddeeff00"), "A138", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Generals" },
                    { new Guid("56700000-5566-7788-99aa-bbccddeeff00"), "A180", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Depreciation, Amortization & Impairment" },
                    { new Guid("56800000-5566-7788-99aa-bbccddeeff00"), "A181", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Bad Debts" },
                    { new Guid("56900000-5566-7788-99aa-bbccddeeff00"), "A182", null, null, false, new Guid("56000000-5566-7788-99aa-bbccddeeff00"), null, null, "Unrealized Losses" }
                });
        }
    }
}
