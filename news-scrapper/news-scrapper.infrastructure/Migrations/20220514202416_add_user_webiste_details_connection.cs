using Microsoft.EntityFrameworkCore.Migrations;

namespace news_scrapper.infrastructure.Migrations
{
    public partial class add_user_webiste_details_connection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WebsitesDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebsitesDetails_UserId",
                table: "WebsitesDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsitesDetails_Users_UserId",
                table: "WebsitesDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebsitesDetails_Users_UserId",
                table: "WebsitesDetails");

            migrationBuilder.DropIndex(
                name: "IX_WebsitesDetails_UserId",
                table: "WebsitesDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WebsitesDetails");
        }
    }
}
