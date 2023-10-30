using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class ChangeColumnType6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_Products_ProductId",
                table: "PaidOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaidOrders_ProductId",
                table: "PaidOrders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PaidOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "PaidOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaidOrders_ProductId",
                table: "PaidOrders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_Products_ProductId",
                table: "PaidOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
