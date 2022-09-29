using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddDedcutionAccountInPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Deduction",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "DeductionAccountId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OtherDeductionAccountId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OtherDeductionAccountId",
                table: "Payments",
                column: "OtherDeductionAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Level4_OtherDeductionAccountId",
                table: "Payments",
                column: "OtherDeductionAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Level4_OtherDeductionAccountId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OtherDeductionAccountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Deduction",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeductionAccountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OtherDeductionAccountId",
                table: "Payments");
        }
    }
}
