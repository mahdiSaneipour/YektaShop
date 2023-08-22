using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BN_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Comments",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Comments");
        }
    }
}
