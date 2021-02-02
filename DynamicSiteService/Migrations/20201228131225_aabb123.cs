using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicSiteService.Migrations
{
    public partial class aabb123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBayi",
                table: "ContentPage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBireysel",
                table: "ContentPage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEndustri",
                table: "ContentPage",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMimar",
                table: "ContentPage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBayi",
                table: "ContentPage");

            migrationBuilder.DropColumn(
                name: "IsBireysel",
                table: "ContentPage");

            migrationBuilder.DropColumn(
                name: "IsEndustri",
                table: "ContentPage");

            migrationBuilder.DropColumn(
                name: "IsMimar",
                table: "ContentPage");
        }
    }
}
