﻿// <auto-generated />
using System;
using Database.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.AzureSql.Migrations
{
    [DbContext(typeof(SqlServerCourseContext))]
    partial class SqlServerCourseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Entities.BookCategoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Selected")
                        .HasColumnType("bit");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SharedCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BookCategories");
                });

            modelBuilder.Entity("Entities.Entities.CheckingHistoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("KidId")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KidId");

                    b.HasIndex("WordId");

                    b.ToTable("CheckingHistories");
                });

            modelBuilder.Entity("Entities.Entities.KidEntity", b =>
                {
                    b.Property<int>("KidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KidId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Selected")
                        .HasColumnType("bit");

                    b.Property<int>("StartSchoolYear")
                        .HasColumnType("int");

                    b.HasKey("KidId");

                    b.ToTable("Kids");
                });

            modelBuilder.Entity("Entities.Entities.WordEntity", b =>
                {
                    b.Property<int>("WordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WordId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Explanation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SharedCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("WordId");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("Entities.Entities.CheckingHistoryEntity", b =>
                {
                    b.HasOne("Entities.Entities.KidEntity", "Kid")
                        .WithMany("CheckingHistories")
                        .HasForeignKey("KidId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Entities.Entities.WordEntity", "Word")
                        .WithMany("CheckingHistories")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Kid");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Entities.Entities.KidEntity", b =>
                {
                    b.Navigation("CheckingHistories");
                });

            modelBuilder.Entity("Entities.Entities.WordEntity", b =>
                {
                    b.Navigation("CheckingHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
