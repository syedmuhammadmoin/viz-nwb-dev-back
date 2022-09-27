using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddingCompositKeyForPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PayrollTransactionMaster_Month_Year_EmployeeId",
                table: "PayrollTransactionMaster",
                columns: new[] { "Month", "Year", "EmployeeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PayrollTransactionMaster_Month_Year_EmployeeId",
                table: "PayrollTransactionMaster");
        }
    }
}
