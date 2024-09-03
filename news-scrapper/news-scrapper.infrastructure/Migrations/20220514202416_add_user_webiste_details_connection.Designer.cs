﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using news_scrapper.infrastructure.DbAccess;

namespace news_scrapper.infrastructure.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    [Migration("20220514202416_add_user_webiste_details_connection")]
    partial class add_user_webiste_details_connection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CategoryDbWebsiteDetailsDb", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("integer");

                    b.Property<int>("Websitesid")
                        .HasColumnType("integer");

                    b.HasKey("CategoriesId", "Websitesid");

                    b.HasIndex("Websitesid");

                    b.ToTable("CategoryDbWebsiteDetailsDb");
                });

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

            modelBuilder.Entity("news_scrapper.domain.DBModels.CategoryDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("news_scrapper.domain.DBModels.UserDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
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

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("UserId");

                    b.ToTable("WebsitesDetails");
                });

            modelBuilder.Entity("CategoryDbWebsiteDetailsDb", b =>
                {
                    b.HasOne("news_scrapper.domain.DBModels.CategoryDb", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("news_scrapper.domain.DBModels.WebsiteDetailsDb", null)
                        .WithMany()
                        .HasForeignKey("Websitesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("news_scrapper.domain.DBModels.UserDb", b =>
                {
                    b.OwnsMany("news_scrapper.domain.DBModels.RefreshTokenDb", "RefreshTokens", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("CreatedByIp")
                                .HasColumnType("text");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("ReasonRevoked")
                                .HasColumnType("text");

                            b1.Property<string>("ReplacedByToken")
                                .HasColumnType("text");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("RevokedByIp")
                                .HasColumnType("text");

                            b1.Property<string>("Token")
                                .HasColumnType("text");

                            b1.Property<int>("UserDbId")
                                .HasColumnType("integer");

                            b1.HasKey("Id");

                            b1.HasIndex("UserDbId");

                            b1.ToTable("RefreshTokens");

                            b1.WithOwner()
                                .HasForeignKey("UserDbId");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("news_scrapper.domain.DBModels.WebsiteDetailsDb", b =>
                {
                    b.HasOne("news_scrapper.domain.DBModels.UserDb", "User")
                        .WithMany("WebsitesDetails")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("news_scrapper.domain.DBModels.UserDb", b =>
                {
                    b.Navigation("WebsitesDetails");
                });

            modelBuilder.Entity("news_scrapper.domain.DBModels.WebsiteDetailsDb", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
