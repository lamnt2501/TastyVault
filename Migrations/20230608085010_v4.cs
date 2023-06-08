using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TastyVault.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuRecipes_AspNetUsers_UserId",
                table: "MenuRecipes");

            migrationBuilder.DropIndex(
                name: "IX_MenuRecipes_UserId",
                table: "MenuRecipes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MenuRecipes");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "MenuRecipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuRecipes_RecipeId",
                table: "MenuRecipes",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRecipes_Recipes_RecipeId",
                table: "MenuRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuRecipes_Recipes_RecipeId",
                table: "MenuRecipes");

            migrationBuilder.DropIndex(
                name: "IX_MenuRecipes_RecipeId",
                table: "MenuRecipes");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "MenuRecipes");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MenuRecipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuRecipes_UserId",
                table: "MenuRecipes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuRecipes_AspNetUsers_UserId",
                table: "MenuRecipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
