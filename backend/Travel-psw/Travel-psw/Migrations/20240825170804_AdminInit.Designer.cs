﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Travel_psw.Data;

#nullable disable

namespace Travel_psw.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240825170804_AdminInit")]
    partial class AdminInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Travel_psw.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Travel_psw.Models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CartId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("TourId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Travel_psw.Models.KeyPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TourId");

                    b.ToTable("KeyPoints");
                });

            modelBuilder.Entity("Travel_psw.Models.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResolvedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.Property<int>("TouristId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Problems", (string)null);
                });

            modelBuilder.Entity("Travel_psw.Models.ProblemEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("character varying(34)");

                    b.Property<DateTime>("OccurredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProblemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ProblemEvents", (string)null);

                    b.HasDiscriminator<string>("EventType").HasValue("ProblemEvent");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Travel_psw.Models.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TourId");

                    b.HasIndex("UserId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Travel_psw.Models.Tour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("Travel_psw.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("Interests")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("InvalidReportCount")
                        .HasColumnType("integer");

                    b.Property<bool>("IsAwarded")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMalicious")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("UnresolvedReviewCount")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Travel_psw.Models.ProblemRejectedEvent", b =>
                {
                    b.HasBaseType("Travel_psw.Models.ProblemEvent");

                    b.HasDiscriminator().HasValue("ProblemRejectedEvent");
                });

            modelBuilder.Entity("Travel_psw.Models.ProblemReportedEvent", b =>
                {
                    b.HasBaseType("Travel_psw.Models.ProblemEvent");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("ProblemReportedEvent");
                });

            modelBuilder.Entity("Travel_psw.Models.ProblemResolvedEvent", b =>
                {
                    b.HasBaseType("Travel_psw.Models.ProblemEvent");

                    b.Property<DateTime>("ResolvedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasDiscriminator().HasValue("ProblemResolvedEvent");
                });

            modelBuilder.Entity("Travel_psw.Models.ProblemSentForReviewEvent", b =>
                {
                    b.HasBaseType("Travel_psw.Models.ProblemEvent");

                    b.HasDiscriminator().HasValue("ProblemSentForReviewEvent");
                });

            modelBuilder.Entity("Travel_psw.Models.Cart", b =>
                {
                    b.HasOne("Travel_psw.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Travel_psw.Models.CartItem", b =>
                {
                    b.HasOne("Travel_psw.Models.Cart", "Cart")
                        .WithMany("Items")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Travel_psw.Models.Tour", "Tour")
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Tour");
                });

            modelBuilder.Entity("Travel_psw.Models.KeyPoint", b =>
                {
                    b.HasOne("Travel_psw.Models.Tour", null)
                        .WithMany("KeyPoints")
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Travel_psw.Models.Sale", b =>
                {
                    b.HasOne("Travel_psw.Models.Tour", "Tour")
                        .WithMany("Sales")
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Travel_psw.Models.User", "User")
                        .WithMany("Sales")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Tour");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Travel_psw.Models.Tour", b =>
                {
                    b.HasOne("Travel_psw.Models.User", "Author")
                        .WithMany("Tours")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Travel_psw.Models.Cart", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Travel_psw.Models.Tour", b =>
                {
                    b.Navigation("KeyPoints");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Travel_psw.Models.User", b =>
                {
                    b.Navigation("Sales");

                    b.Navigation("Tours");
                });
#pragma warning restore 612, 618
        }
    }
}
