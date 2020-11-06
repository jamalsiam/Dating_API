using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class NewPropsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conuntry",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Conuntry",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
