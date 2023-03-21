﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateFixedAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixedAssetId",
                table: "RecordLedger",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AccumulatedDepreciationAmount",
                table: "FixedAssets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalActiveDays",
                table: "FixedAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CashOrAccountsReceivableAccountId",
                table: "Disposals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GainOrLossOnDisposalAccountId",
                table: "Disposals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Disposals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DepreciationRegister",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixedAssetId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAutomatedCalculation = table.Column<bool>(type: "bit", nullable: false),
                    DepreciationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepreciationRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationRegister_FixedAssets_FixedAssetId",
                        column: x => x.FixedAssetId,
                        principalTable: "FixedAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixedAssetLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    ActiveDays = table.Column<int>(type: "int", nullable: false),
                    ActiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InactiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedAssetLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedAssetLines_FixedAssets_MasterId",
                        column: x => x.MasterId,
                        principalTable: "FixedAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordLedger_FixedAssetId",
                table: "RecordLedger",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_CashOrAccountsReceivableAccountId",
                table: "Disposals",
                column: "CashOrAccountsReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_GainOrLossOnDisposalAccountId",
                table: "Disposals",
                column: "GainOrLossOnDisposalAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Disposals_TransactionId",
                table: "Disposals",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationRegister_FixedAssetId",
                table: "DepreciationRegister",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssetLines_MasterId",
                table: "FixedAssetLines",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disposals_Level4_CashOrAccountsReceivableAccountId",
                table: "Disposals",
                column: "CashOrAccountsReceivableAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Disposals_Level4_GainOrLossOnDisposalAccountId",
                table: "Disposals",
                column: "GainOrLossOnDisposalAccountId",
                principalTable: "Level4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Disposals_Transactions_TransactionId",
                table: "Disposals",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLedger_FixedAssets_FixedAssetId",
                table: "RecordLedger",
                column: "FixedAssetId",
                principalTable: "FixedAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disposals_Level4_CashOrAccountsReceivableAccountId",
                table: "Disposals");

            migrationBuilder.DropForeignKey(
                name: "FK_Disposals_Level4_GainOrLossOnDisposalAccountId",
                table: "Disposals");

            migrationBuilder.DropForeignKey(
                name: "FK_Disposals_Transactions_TransactionId",
                table: "Disposals");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLedger_FixedAssets_FixedAssetId",
                table: "RecordLedger");

            migrationBuilder.DropTable(
                name: "DepreciationRegister");

            migrationBuilder.DropTable(
                name: "FixedAssetLines");

            migrationBuilder.DropIndex(
                name: "IX_RecordLedger_FixedAssetId",
                table: "RecordLedger");

            migrationBuilder.DropIndex(
                name: "IX_Disposals_CashOrAccountsReceivableAccountId",
                table: "Disposals");

            migrationBuilder.DropIndex(
                name: "IX_Disposals_GainOrLossOnDisposalAccountId",
                table: "Disposals");

            migrationBuilder.DropIndex(
                name: "IX_Disposals_TransactionId",
                table: "Disposals");

            migrationBuilder.DropColumn(
                name: "FixedAssetId",
                table: "RecordLedger");

            migrationBuilder.DropColumn(
                name: "AccumulatedDepreciationAmount",
                table: "FixedAssets");

            migrationBuilder.DropColumn(
                name: "TotalActiveDays",
                table: "FixedAssets");

            migrationBuilder.DropColumn(
                name: "CashOrAccountsReceivableAccountId",
                table: "Disposals");

            migrationBuilder.DropColumn(
                name: "GainOrLossOnDisposalAccountId",
                table: "Disposals");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Disposals");
        }
    }
}