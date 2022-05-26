using Microsoft.EntityFrameworkCore.Migrations;

namespace Magicodes.Admin.EntityFrameworkCore.Migrations
{
    public partial class Update_MoYuRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "MoYuRecords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TogetherWxUsers",
                table: "MoYuRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "MoYuRecords");

            migrationBuilder.DropColumn(
                name: "TogetherWxUsers",
                table: "MoYuRecords");
        }
    }
}
