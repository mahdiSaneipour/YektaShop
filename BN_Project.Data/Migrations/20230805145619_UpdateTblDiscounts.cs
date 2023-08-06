using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BN_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Discounts");
        }
    }
}
