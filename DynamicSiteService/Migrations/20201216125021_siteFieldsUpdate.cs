using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicSiteService.Migrations
{
    public partial class siteFieldsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInteral",
                table: "ContentPage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInterax",
                table: "ContentPage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIntersecure",
                table: "ContentPage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInterwall",
                table: "ContentPage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInteral",
                table: "ContentPage");

            migrationBuilder.DropColumn(
                name: "IsInterax",
                table: "ContentPage");

            migrationBuilder.DropColumn(
                name: "IsIntersecure",
                table: "ContentPage");

            migrationBuilder.DropColumn(
                name: "IsInterwall",
                table: "ContentPage");
        }
    }
}
