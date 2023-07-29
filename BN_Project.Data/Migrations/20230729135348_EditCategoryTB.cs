using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BN_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditCategoryTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catagories_Catagories_ParentId",
                table: "Catagories");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Catagories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Catagories_Catagories_ParentId",
                table: "Catagories",
                column: "ParentId",
                principalTable: "Catagories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catagories_Catagories_ParentId",
                table: "Catagories");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Catagories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Catagories_Catagories_ParentId",
                table: "Catagories",
                column: "ParentId",
                principalTable: "Catagories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
