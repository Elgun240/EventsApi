using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class ChangeColumnType5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_AspNetUsers_AppUserId1",
                table: "PaidOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaidOrders_AppUserId1",
                table: "PaidOrders");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PaidOrders");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PaidOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PaidOrders_AppUserId",
                table: "PaidOrders",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_AspNetUsers_AppUserId",
                table: "PaidOrders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaidOrders_AspNetUsers_AppUserId",
                table: "PaidOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaidOrders_AppUserId",
                table: "PaidOrders");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "PaidOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PaidOrders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaidOrders_AppUserId1",
                table: "PaidOrders",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PaidOrders_AspNetUsers_AppUserId1",
                table: "PaidOrders",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
