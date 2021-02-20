using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class postAddFeeling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Feeling",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feeling",
                table: "Posts");
        }
    }
}
