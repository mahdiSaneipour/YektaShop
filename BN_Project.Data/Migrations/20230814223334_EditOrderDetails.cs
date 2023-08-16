using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BN_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FinalPrice",
                table: "OrderDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "OrderDetails");
        }
    }
}
