using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class ChangeRestauranColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenDays",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OpeningHours",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "OpenHours",
                table: "Restaurants",
                newName: "OpeningTime");

            migrationBuilder.RenameColumn(
                name: "CloseHours",
                table: "Restaurants",
                newName: "ClosingTime");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDays",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "OpeningTime",
                table: "Restaurants",
                newName: "OpenHours");

            migrationBuilder.RenameColumn(
                name: "ClosingTime",
                table: "Restaurants",
                newName: "CloseHours");

            migrationBuilder.AddColumn<int>(
                name: "OpenDays",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpeningHours",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
