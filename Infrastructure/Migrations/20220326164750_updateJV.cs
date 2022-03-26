using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class updateJV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "JournalEntryMaster",
                newName: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryMaster_StatusId",
                table: "JournalEntryMaster",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntryMaster_WorkFlowStatus_StatusId",
                table: "JournalEntryMaster",
                column: "StatusId",
                principalTable: "WorkFlowStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntryMaster_WorkFlowStatus_StatusId",
                table: "JournalEntryMaster");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntryMaster_StatusId",
                table: "JournalEntryMaster");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "JournalEntryMaster",
                newName: "Status");
        }
    }
}
