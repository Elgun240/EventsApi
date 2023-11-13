using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class AddRestaurantColumnToRestauratnTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "PaidOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaidOrders_RestaurantId",
                table: "PaidOrders",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_Restaurants_RestaurantId",
                table: "PaidOrders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_Restaurants_RestaurantId",
                table: "PaidOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaidOrders_RestaurantId",
                table: "PaidOrders");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "PaidOrders");
        }
    }
}
