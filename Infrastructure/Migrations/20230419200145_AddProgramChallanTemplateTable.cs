using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddProgramChallanTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgramChallanTemplateMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramChallanTypeMyProperty = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: true),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LateFeeAfterDueDate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChallanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramChallanTemplateMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateMaster_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateMaster_Level4_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "Level4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateMaster_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateMaster_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateMaster_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramChallanTemplateLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeItemId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramChallanTemplateLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateLines_FeeItems_FeeItemId",
                        column: x => x.FeeItemId,
                        principalTable: "FeeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramChallanTemplateLines_ProgramChallanTemplateMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "ProgramChallanTemplateMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateLines_FeeItemId",
                table: "ProgramChallanTemplateLines",
                column: "FeeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateLines_MasterId",
                table: "ProgramChallanTemplateLines",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateMaster_BankAccountId",
                table: "ProgramChallanTemplateMaster",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateMaster_CampusId",
                table: "ProgramChallanTemplateMaster",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateMaster_ProgramId",
                table: "ProgramChallanTemplateMaster",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateMaster_SemesterId",
                table: "ProgramChallanTemplateMaster",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramChallanTemplateMaster_ShiftId",
                table: "ProgramChallanTemplateMaster",
                column: "ShiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramChallanTemplateLines");

            migrationBuilder.DropTable(
                name: "ProgramChallanTemplateMaster");
        }
    }
}
