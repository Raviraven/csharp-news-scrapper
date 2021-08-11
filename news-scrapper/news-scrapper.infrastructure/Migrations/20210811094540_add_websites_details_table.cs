using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace news_scrapper.infrastructure.Migrations
{
    public partial class add_websites_details_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsitesDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: true),
                    MainNodeXPathToNewsContainer = table.Column<string>(type: "text", nullable: true),
                    NewsNodeTag = table.Column<string>(type: "text", nullable: true),
                    NewsNodeClass = table.Column<string>(type: "text", nullable: true),
                    TitleNodeTag = table.Column<string>(type: "text", nullable: true),
                    TitleNodeClass = table.Column<string>(type: "text", nullable: true),
                    DescriptionNodeTag = table.Column<string>(type: "text", nullable: true),
                    DescriptionNodeClass = table.Column<string>(type: "text", nullable: true),
                    ImgNodeClass = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsitesDetails", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsitesDetails");
        }
    }
}
