using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class AddOrderColumnToPaidORders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "PaidOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaidOrders_OrderId",
                table: "PaidOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_Orders_OrderId",
                table: "PaidOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_Orders_OrderId",
                table: "PaidOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaidOrders_OrderId",
                table: "PaidOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PaidOrders");
        }
    }
}
