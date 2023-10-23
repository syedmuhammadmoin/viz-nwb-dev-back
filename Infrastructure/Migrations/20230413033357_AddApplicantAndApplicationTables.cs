using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddApplicantAndApplicationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicantId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "FixedAssets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "CWIPs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlaceOfBirthId = table.Column<int>(type: "int", nullable: true),
                    PostalAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Religion = table.Column<int>(type: "int", nullable: true),
                    DomicileId = table.Column<int>(type: "int", nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicants_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicants_Cities_PlaceOfBirthId",
                        column: x => x.PlaceOfBirthId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicants_Countries_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicants_Domiciles_DomicileId",
                        column: x => x.DomicileId,
                        principalTable: "Domiciles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdmissionApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdmissionCriteriaId = table.Column<int>(type: "int", nullable: true),
                    IsEntryTestRequired = table.Column<bool>(type: "bit", nullable: true),
                    EntryTestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EntryTestAttendance = table.Column<int>(type: "int", nullable: true),
                    EntryTestMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EntryTestRequriedMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsInterviewRequired = table.Column<bool>(type: "bit", nullable: true),
                    InterviewAttendance = table.Column<int>(type: "int", nullable: true),
                    InterviewStatus = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_AdmissionCriteria_AdmissionCriteriaId",
                        column: x => x.AdmissionCriteriaId,
                        principalTable: "AdmissionCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_BatchMaster_BatchId",
                        column: x => x.BatchId,
                        principalTable: "BatchMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplications_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualificationId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    PassingYear = table.Column<int>(type: "int", nullable: false),
                    MarksOrGPA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InstituteOrBoard = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantQualifications_Applicants_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantQualifications_Qualifications_QualificationId",
                        column: x => x.QualificationId,
                        principalTable: "Qualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantQualifications_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantRelatives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Relationship = table.Column<int>(type: "int", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantRelatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantRelatives_Applicants_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdmissionApplicationHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdmissionApplicationId = table.Column<int>(type: "int", nullable: false),
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionApplicationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionApplicationHistories_AdmissionApplications_AdmissionApplicationId",
                        column: x => x.AdmissionApplicationId,
                        principalTable: "AdmissionApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionApplicationHistories_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApplicantId",
                table: "Users",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_EmployeeId",
                table: "FixedAssets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CWIPs_TransactionId",
                table: "CWIPs",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplicationHistories_AdmissionApplicationId",
                table: "AdmissionApplicationHistories",
                column: "AdmissionApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplicationHistories_BusinessPartnerId",
                table: "AdmissionApplicationHistories",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_AdmissionCriteriaId",
                table: "AdmissionApplications",
                column: "AdmissionCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_ApplicantId",
                table: "AdmissionApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_BatchId",
                table: "AdmissionApplications",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_CampusId",
                table: "AdmissionApplications",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_ProgramId",
                table: "AdmissionApplications",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionApplications_ShiftId",
                table: "AdmissionApplications",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantQualifications_MasterId",
                table: "ApplicantQualifications",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantQualifications_QualificationId",
                table: "ApplicantQualifications",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantQualifications_SubjectId",
                table: "ApplicantQualifications",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantRelatives_MasterId",
                table: "ApplicantRelatives",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_BusinessPartnerId",
                table: "Applicants",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_DomicileId",
                table: "Applicants",
                column: "DomicileId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_NationalityId",
                table: "Applicants",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_PlaceOfBirthId",
                table: "Applicants",
                column: "PlaceOfBirthId");

            migrationBuilder.AddForeignKey(
                name: "FK_CWIPs_Transactions_TransactionId",
                table: "CWIPs",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixedAssets_Employees_EmployeeId",
                table: "FixedAssets",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Applicants_ApplicantId",
                table: "Users",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CWIPs_Transactions_TransactionId",
                table: "CWIPs");

            migrationBuilder.DropForeignKey(
                name: "FK_FixedAssets_Employees_EmployeeId",
                table: "FixedAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Applicants_ApplicantId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AdmissionApplicationHistories");

            migrationBuilder.DropTable(
                name: "ApplicantQualifications");

            migrationBuilder.DropTable(
                name: "ApplicantRelatives");

            migrationBuilder.DropTable(
                name: "AdmissionApplications");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Users_ApplicantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_FixedAssets_EmployeeId",
                table: "FixedAssets");

            migrationBuilder.DropIndex(
                name: "IX_CWIPs_TransactionId",
                table: "CWIPs");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "FixedAssets");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "CWIPs");
        }
    }
}
