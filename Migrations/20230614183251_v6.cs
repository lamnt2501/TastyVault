using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyVault.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenusImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "ntext", nullable: false),
                    MenusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenusImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenusImages_Menus_MenusId",
                        column: x => x.MenusId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenusImages_MenusId",
                table: "MenusImages",
                column: "MenusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenusImages");
        }
    }
}
