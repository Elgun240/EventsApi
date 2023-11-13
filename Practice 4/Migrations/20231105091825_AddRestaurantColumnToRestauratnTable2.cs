using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class AddRestaurantColumnToRestauratnTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_Restaurants_RestaurantId",
                table: "PaidOrders");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "PaidOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_Restaurants_RestaurantId",
                table: "PaidOrders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_Restaurants_RestaurantId",
                table: "PaidOrders");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "PaidOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_Restaurants_RestaurantId",
                table: "PaidOrders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
