﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Shoppingify;

#nullable disable

namespace Shoppingify.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231014191540_ModifyCartOwner")]
    partial class ModifyCartOwner
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shoppingify.Cart.Domain.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("CartOwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("CartOwnerId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Shoppingify.Cart.Domain.CartOwner", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<Guid?>("ActiveCart")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("CartOwners");
                });

            modelBuilder.Entity("Shoppingify.Cart.Domain.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProductId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Shoppingify.Products.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("Owner");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Shoppingify.Cart.Domain.Cart", b =>
                {
                    b.OwnsMany("Shoppingify.Cart.Domain.CartItem", "CartItems", b1 =>
                        {
                            b1.Property<Guid>("CartId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<int>("Status")
                                .HasColumnType("integer");

                            b1.HasKey("CartId", "Id");

                            b1.HasIndex("ProductId");

                            b1.ToTable("CartItem");

                            b1.WithOwner()
                                .HasForeignKey("CartId");

                            b1.HasOne("Shoppingify.Cart.Domain.Product", "Product")
                                .WithMany()
                                .HasForeignKey("ProductId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Product");
                        });

                    b.Navigation("CartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
