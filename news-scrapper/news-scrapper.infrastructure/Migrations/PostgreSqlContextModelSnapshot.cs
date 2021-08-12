﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using news_scrapper.infrastructure.DbAccess;

namespace news_scrapper.infrastructure.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    partial class PostgreSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("news_scrapper.domain.DBModels.ArticleDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateScrapped")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<int>("WebsiteDetailsId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteDetailsId");

                    b.ToTable("Articles");
                });

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

            modelBuilder.Entity("news_scrapper.domain.DBModels.ArticleDb", b =>
                {
                    b.HasOne("news_scrapper.domain.DBModels.WebsiteDetailsDb", "WebsiteDetails")
                        .WithMany("Articles")
                        .HasForeignKey("WebsiteDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WebsiteDetails");
                });

            modelBuilder.Entity("news_scrapper.domain.DBModels.WebsiteDetailsDb", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
