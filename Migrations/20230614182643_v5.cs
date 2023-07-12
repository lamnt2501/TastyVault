using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyVault.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path",
                table: "Menus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
