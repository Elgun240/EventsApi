﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practice_4.Migrations
{
    public partial class AddAdressColumnToPaidOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "PaidOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "PaidOrders");
        }
    }
}
