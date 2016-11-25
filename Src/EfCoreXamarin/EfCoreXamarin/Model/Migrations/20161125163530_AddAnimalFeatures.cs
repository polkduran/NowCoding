using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddAnimalFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Animal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Animal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FurSoftness",
                table: "Animal",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BeakPower",
                table: "Animal",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "FurSoftness",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "BeakPower",
                table: "Animal");
        }
    }
}
