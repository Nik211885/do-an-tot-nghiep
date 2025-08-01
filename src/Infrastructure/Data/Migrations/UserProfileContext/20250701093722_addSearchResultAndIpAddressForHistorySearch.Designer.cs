﻿// <auto-generated />
using System;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations.UserProfileContext
{
    [DbContext(typeof(UserProfileDbContext))]
    [Migration("20250701093722_addSearchResultAndIpAddressForHistorySearch")]
    partial class addSearchResultAndIpAddressForHistorySearch
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.FavoriteBookAggregate.FavoriteBook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FavoriteBookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteBook", "UserProfile");
                });

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.FollowerAggregate.Follower", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("FollowDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FollowingId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FollowerId");

                    b.HasIndex("FollowingId");

                    b.ToTable("Follower", "UserProfile");
                });

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.SearchHistoryAggregate.SearchHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("SearchCout")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("SearchDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SearchTerm")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SearchHistory", "UserProfile");
                });

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bio")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("CountFavoriteBook")
                        .HasColumnType("integer");

                    b.Property<int>("CountFollowing")
                        .HasColumnType("integer");

                    b.Property<int>("CoutFollowers")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserProfile", "UserProfile");
                });

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.FavoriteBookAggregate.FavoriteBook", b =>
                {
                    b.HasOne("Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.FollowerAggregate.Follower", b =>
                {
                    b.HasOne("Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.BoundContext.UserProfileContext.SearchHistoryAggregate.SearchHistory", b =>
                {
                    b.HasOne("Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
