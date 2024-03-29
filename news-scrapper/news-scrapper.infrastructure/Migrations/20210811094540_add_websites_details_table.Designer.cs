﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using news_scrapper.infrastructure.DbAccess;

namespace news_scrapper.infrastructure.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    [Migration("20210811094540_add_websites_details_table")]
    partial class add_websites_details_table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("news_scrapper.domain.DBModels.WebsiteDetailsDb", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("DescriptionNodeClass")
                        .HasColumnType("text");

                    b.Property<string>("DescriptionNodeTag")
                        .HasColumnType("text");

                    b.Property<string>("ImgNodeClass")
                        .HasColumnType("text");

                    b.Property<string>("MainNodeXPathToNewsContainer")
                        .HasColumnType("text");

                    b.Property<string>("NewsNodeClass")
                        .HasColumnType("text");

                    b.Property<string>("NewsNodeTag")
                        .HasColumnType("text");

                    b.Property<string>("TitleNodeClass")
                        .HasColumnType("text");

                    b.Property<string>("TitleNodeTag")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("WebsitesDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
