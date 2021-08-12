using Microsoft.EntityFrameworkCore.Migrations;

namespace news_scrapper.infrastructure.Migrations
{
    public partial class add_website_details_field_into_articles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_WebsitesDetails_WebsiteDetailsid",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "WebsiteDetailsid",
                table: "Articles",
                newName: "WebsiteDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_WebsiteDetailsid",
                table: "Articles",
                newName: "IX_Articles_WebsiteDetailsId");

            migrationBuilder.AlterColumn<int>(
                name: "WebsiteDetailsId",
                table: "Articles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_WebsitesDetails_WebsiteDetailsId",
                table: "Articles",
                column: "WebsiteDetailsId",
                principalTable: "WebsitesDetails",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_WebsitesDetails_WebsiteDetailsId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "WebsiteDetailsId",
                table: "Articles",
                newName: "WebsiteDetailsid");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_WebsiteDetailsId",
                table: "Articles",
                newName: "IX_Articles_WebsiteDetailsid");

            migrationBuilder.AlterColumn<int>(
                name: "WebsiteDetailsid",
                table: "Articles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_WebsitesDetails_WebsiteDetailsid",
                table: "Articles",
                column: "WebsiteDetailsid",
                principalTable: "WebsitesDetails",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
