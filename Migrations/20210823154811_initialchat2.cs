using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class initialchat2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Connections");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Connections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Connections");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Connections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
