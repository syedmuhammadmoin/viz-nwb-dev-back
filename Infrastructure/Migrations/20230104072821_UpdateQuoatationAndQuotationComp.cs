using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateQuoatationAndQuotationComp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAwarded",
                table: "QuotationMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AwardedVendor",
                table: "QuotationComparativeMaster",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAwarded",
                table: "QuotationMaster");

            migrationBuilder.DropColumn(
                name: "AwardedVendor",
                table: "QuotationComparativeMaster");
        }
    }
}
