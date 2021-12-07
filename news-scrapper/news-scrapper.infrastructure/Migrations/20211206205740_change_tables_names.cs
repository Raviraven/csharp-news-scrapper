using Microsoft.EntityFrameworkCore.Migrations;

namespace news_scrapper.infrastructure.Migrations
{
    public partial class change_tables_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDbWebsiteDetailsDb_CategoryDb_CategoriesId",
                table: "CategoryDbWebsiteDetailsDb");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokenDb_Users_UserDbId",
                table: "RefreshTokenDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokenDb",
                table: "RefreshTokenDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDb",
                table: "CategoryDb");

            migrationBuilder.RenameTable(
                name: "RefreshTokenDb",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "CategoryDb",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokenDb_UserDbId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserDbId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDbWebsiteDetailsDb_Categories_CategoriesId",
                table: "CategoryDbWebsiteDetailsDb",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserDbId",
                table: "RefreshTokens",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDbWebsiteDetailsDb_Categories_CategoriesId",
                table: "CategoryDbWebsiteDetailsDb");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserDbId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshTokenDb");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "CategoryDb");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserDbId",
                table: "RefreshTokenDb",
                newName: "IX_RefreshTokenDb_UserDbId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokenDb",
                table: "RefreshTokenDb",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDb",
                table: "CategoryDb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDbWebsiteDetailsDb_CategoryDb_CategoriesId",
                table: "CategoryDbWebsiteDetailsDb",
                column: "CategoriesId",
                principalTable: "CategoryDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokenDb_Users_UserDbId",
                table: "RefreshTokenDb",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
