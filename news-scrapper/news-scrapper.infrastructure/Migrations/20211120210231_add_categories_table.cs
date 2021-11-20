using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace news_scrapper.infrastructure.Migrations
{
    public partial class add_categories_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryDbWebsiteDetailsDb",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    Websitesid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDbWebsiteDetailsDb", x => new { x.CategoriesId, x.Websitesid });
                    table.ForeignKey(
                        name: "FK_CategoryDbWebsiteDetailsDb_CategoryDb_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "CategoryDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryDbWebsiteDetailsDb_WebsitesDetails_Websitesid",
                        column: x => x.Websitesid,
                        principalTable: "WebsitesDetails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDbWebsiteDetailsDb_Websitesid",
                table: "CategoryDbWebsiteDetailsDb",
                column: "Websitesid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryDbWebsiteDetailsDb");

            migrationBuilder.DropTable(
                name: "CategoryDb");
        }
    }
}
