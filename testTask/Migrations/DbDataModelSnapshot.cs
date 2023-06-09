﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using testTask.Data;

#nullable disable

namespace testTask.Migrations
{
    [DbContext(typeof(DbData))]
    partial class DbDataModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("testTask.Models.CoursesOfPersonal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("InformationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("InformationId");

                    b.ToTable("coursesOfPersonals");
                });

            modelBuilder.Entity("testTask.Models.Information", b =>
                {
                    b.Property<int>("InfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("InfoId"));

                    b.Property<string>("AcademicAchievement")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Brith")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CourseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CourseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MilitaryRank")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StatisticalNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("InfoId");

                    b.ToTable("informations");
                });

            modelBuilder.Entity("testTask.Models.PromotionsOfPersonal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("InformationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("InformationId");

                    b.ToTable("promotionsOfPersonals");
                });

            modelBuilder.Entity("testTask.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("testTask.Models.CoursesOfPersonal", b =>
                {
                    b.HasOne("testTask.Models.Information", "Information")
                        .WithMany("coursesOfPersonals")
                        .HasForeignKey("InformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Information");
                });

            modelBuilder.Entity("testTask.Models.PromotionsOfPersonal", b =>
                {
                    b.HasOne("testTask.Models.Information", "Information")
                        .WithMany("promotionsOfPersonals")
                        .HasForeignKey("InformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Information");
                });

            modelBuilder.Entity("testTask.Models.Information", b =>
                {
                    b.Navigation("coursesOfPersonals");

                    b.Navigation("promotionsOfPersonals");
                });
#pragma warning restore 612, 618
        }
    }
}
