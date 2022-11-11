using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddAllEmpFieldsInPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NetIncrement",
                table: "PayrollTransactionMaster");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTitle",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PayrollTransactionMaster",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BasicPayItemId",
                table: "PayrollTransactionMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "PayrollTransactionMaster",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CasualLeaves",
                table: "PayrollTransactionMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "PayrollTransactionMaster",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofBirth",
                table: "PayrollTransactionMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofJoining",
                table: "PayrollTransactionMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofRetirment",
                table: "PayrollTransactionMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Domicile",
                table: "PayrollTransactionMaster",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DutyShift",
                table: "PayrollTransactionMaster",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EarnedLeaves",
                table: "PayrollTransactionMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PayrollTransactionMaster",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "PayrollTransactionMaster",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeType",
                table: "PayrollTransactionMaster",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "PayrollTransactionMaster",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "PayrollTransactionMaster",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "PayrollTransactionMaster",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncrementItemId",
                table: "PayrollTransactionMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Maritalstatus",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PayrollTransactionMaster",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfIncrements",
                table: "PayrollTransactionMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceofBirth",
                table: "PayrollTransactionMaster",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "PayrollTransactionMaster",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_BasicPayItemId",
                table: "PayrollTransactionMaster",
                column: "BasicPayItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTransactionMaster_IncrementItemId",
                table: "PayrollTransactionMaster",
                column: "IncrementItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollTransactionMaster_PayrollItems_BasicPayItemId",
                table: "PayrollTransactionMaster",
                column: "BasicPayItemId",
                principalTable: "PayrollItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollTransactionMaster_PayrollItems_IncrementItemId",
                table: "PayrollTransactionMaster",
                column: "IncrementItemId",
                principalTable: "PayrollItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollTransactionMaster_PayrollItems_BasicPayItemId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollTransactionMaster_PayrollItems_IncrementItemId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropIndex(
                name: "IX_PayrollTransactionMaster_BasicPayItemId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropIndex(
                name: "IX_PayrollTransactionMaster_IncrementItemId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "AccountTitle",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "BasicPayItemId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "CasualLeaves",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "DateofBirth",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "DateofJoining",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "DateofRetirment",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Domicile",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "DutyShift",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "EarnedLeaves",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "EmployeeType",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "IncrementItemId",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Maritalstatus",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "NoOfIncrements",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "PlaceofBirth",
                table: "PayrollTransactionMaster");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "PayrollTransactionMaster");

            migrationBuilder.AddColumn<decimal>(
                name: "NetIncrement",
                table: "PayrollTransactionMaster",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
