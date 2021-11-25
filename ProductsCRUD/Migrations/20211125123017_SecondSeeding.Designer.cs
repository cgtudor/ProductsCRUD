﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductsCRUD.Context;

namespace ProductsCRUD.Migrations
{
    [DbContext(typeof(Context.Context))]
    [Migration("20211125123017_SecondSeeding")]
    partial class SecondSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductsCRUD.DomainModels.ProductDomainModel", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ProductPrice")
                        .HasColumnType("float");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.ToTable("_products");

                    b.HasData(
                        new
                        {
                            ProductID = -1,
                            ProductDescription = "Tasty and savory.",
                            ProductName = "Chocolate",
                            ProductPrice = 1.5600000000000001,
                            ProductQuantity = 4
                        },
                        new
                        {
                            ProductID = -2,
                            ProductDescription = "Blonde beer. Yummy.",
                            ProductName = "Heineken 0.5L",
                            ProductPrice = 2.5600000000000001,
                            ProductQuantity = 45
                        },
                        new
                        {
                            ProductID = -3,
                            ProductDescription = "Fresh.",
                            ProductName = "Bread",
                            ProductPrice = 0.56000000000000005,
                            ProductQuantity = 1243
                        },
                        new
                        {
                            ProductID = -4,
                            ProductDescription = "Pack of 50 nails.",
                            ProductName = "Nails",
                            ProductPrice = 4.5,
                            ProductQuantity = 23
                        },
                        new
                        {
                            ProductID = -5,
                            ProductDescription = "Aroma like you've never smelled before.",
                            ProductName = "Candle",
                            ProductPrice = 3.5600000000000001,
                            ProductQuantity = 43
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
