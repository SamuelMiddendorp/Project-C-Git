using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TheRichLifeProject.Migrations
{
    public partial class ProductModelupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "ShortDescription");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Mature",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Mature",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Products",
                newName: "Description");
        }
    }
}
