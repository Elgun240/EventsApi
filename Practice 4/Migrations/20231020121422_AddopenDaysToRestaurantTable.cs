using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class AddopenDaysToRestaurantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OenDays",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OenDays",
                table: "Restaurants");
        }
    }
}
