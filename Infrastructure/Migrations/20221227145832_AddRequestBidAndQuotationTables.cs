using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddRequestBidAndQuotationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservedRequisitionQuantity",
                table: "Stock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithoutWorkflow",
                table: "RequisitionMaster",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "RequisitionMaster",
                type: "int",
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

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "RequisitionLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReserveQuantity",
                table: "RequisitionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BidEvaluationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MethodOfProcurement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TendorInquiryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberOfBids = table.Column<int>(type: "int", nullable: false),
                    DateOfOpeningBid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfClosingBid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BidEvaluationCriteria = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LowestEvaluatedBidder = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidEvaluationMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallForQuotationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CallForQuotationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallForQuotationMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallForQuotationMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuotationComparativeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    QuotationComparativeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequsisitionId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationComparativeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationComparativeMaster_RequisitionMaster_RequsisitionId",
                        column: x => x.RequsisitionId,
                        principalTable: "RequisitionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestMaster_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BidEvaluationLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOfBider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnicalTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TechnicalObtain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinancialObtain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EvaluatedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rule = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidEvaluationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BidEvaluationLines_BidEvaluationMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "BidEvaluationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CallForQuotationLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallForQuotationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallForQuotationLines_CallForQuotationMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "CallForQuotationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CallForQuotationLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuotationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    QuotationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    Timeframe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequisitionId = table.Column<int>(type: "int", nullable: true),
                    QuotationComparativeId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationMaster_BusinessPartners_VendorId",
                        column: x => x.VendorId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationMaster_QuotationComparativeMaster_QuotationComparativeId",
                        column: x => x.QuotationComparativeId,
                        principalTable: "QuotationComparativeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationMaster_WorkFlowStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "WorkFlowStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestLines_RequestMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "RequestMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationLines_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationLines_QuotationMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "QuotationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BidEvaluationLines_MasterId",
                table: "BidEvaluationLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CallForQuotationLines_ItemId",
                table: "CallForQuotationLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CallForQuotationLines_MasterId",
                table: "CallForQuotationLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CallForQuotationMaster_VendorId",
                table: "CallForQuotationMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationComparativeMaster_RequsisitionId",
                table: "QuotationComparativeMaster",
                column: "RequsisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLines_ItemId",
                table: "QuotationLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLines_MasterId",
                table: "QuotationLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationMaster_QuotationComparativeId",
                table: "QuotationMaster",
                column: "QuotationComparativeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationMaster_StatusId",
                table: "QuotationMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationMaster_VendorId",
                table: "QuotationMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLines_MasterId",
                table: "RequestLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMaster_CampusId",
                table: "RequestMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMaster_EmployeeId",
                table: "RequestMaster",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMaster_StatusId",
                table: "RequestMaster",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidEvaluationLines");

            migrationBuilder.DropTable(
                name: "CallForQuotationLines");

            migrationBuilder.DropTable(
                name: "QuotationLines");

            migrationBuilder.DropTable(
                name: "RequestLines");

            migrationBuilder.DropTable(
                name: "BidEvaluationMaster");

            migrationBuilder.DropTable(
                name: "CallForQuotationMaster");

            migrationBuilder.DropTable(
                name: "QuotationMaster");

            migrationBuilder.DropTable(
                name: "RequestMaster");

            migrationBuilder.DropTable(
                name: "QuotationComparativeMaster");

            migrationBuilder.DropColumn(
                name: "ReservedRequisitionQuantity",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "IsWithoutWorkflow",
                table: "RequisitionMaster");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "RequisitionMaster");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "RequisitionLines");

            migrationBuilder.DropColumn(
                name: "ReserveQuantity",
                table: "RequisitionLines");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "RequisitionLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
