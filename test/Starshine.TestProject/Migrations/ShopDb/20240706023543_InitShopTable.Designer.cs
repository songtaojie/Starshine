﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Starshine.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Test;

#nullable disable

namespace Starshine.TestProject.Migrations.ShopDb
{
    [DbContext(typeof(ShopDbContext))]
    [Migration("20240706023543_InitShopTable")]
    partial class InitShopTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Starshine_DatabaseProvider", EFCoreDatabaseProvider.MySql)
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Starshine.EntityFrameworkCore.Test.Entities.Shop", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("DeleterId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ShopName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("UpdaterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Shop");
                });
#pragma warning restore 612, 618
        }
    }
}
