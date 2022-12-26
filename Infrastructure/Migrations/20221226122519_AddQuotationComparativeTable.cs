using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddQuotationComparativeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CallForQuotationId",
                table: "QuotationMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuotationComparativeId",
                table: "QuotationMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuotationComparativeMasterId",
                table: "QuotationMasters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuotationComparativeMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequsisitionId = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    QuotationComparativeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationComparativeMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationComparativeMasters_RequisitionMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "RequisitionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuotationComparativeLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QoutationIds = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationComparativeLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationComparativeLines_QuotationComparativeMasters_MasterId",
                        column: x => x.MasterId,
                        principalTable: "QuotationComparativeMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationComparativeLines_QuotationMasters_QoutationIds",
                        column: x => x.QoutationIds,
                        principalTable: "QuotationMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationMasters_QuotationComparativeMasterId",
                table: "QuotationMasters",
                column: "QuotationComparativeMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationComparativeLines_MasterId",
                table: "QuotationComparativeLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationComparativeLines_QoutationIds",
                table: "QuotationComparativeLines",
                column: "QoutationIds");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationComparativeMasters_MasterId",
                table: "QuotationComparativeMasters",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationMasters_QuotationComparativeMasters_QuotationComparativeMasterId",
                table: "QuotationMasters",
                column: "QuotationComparativeMasterId",
                principalTable: "QuotationComparativeMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationMasters_QuotationComparativeMasters_QuotationComparativeMasterId",
                table: "QuotationMasters");

            migrationBuilder.DropTable(
                name: "QuotationComparativeLines");

            migrationBuilder.DropTable(
                name: "QuotationComparativeMasters");

            migrationBuilder.DropIndex(
                name: "IX_QuotationMasters_QuotationComparativeMasterId",
                table: "QuotationMasters");

            migrationBuilder.DropColumn(
                name: "CallForQuotationId",
                table: "QuotationMasters");

            migrationBuilder.DropColumn(
                name: "QuotationComparativeId",
                table: "QuotationMasters");

            migrationBuilder.DropColumn(
                name: "QuotationComparativeMasterId",
                table: "QuotationMasters");
        }
    }
}
