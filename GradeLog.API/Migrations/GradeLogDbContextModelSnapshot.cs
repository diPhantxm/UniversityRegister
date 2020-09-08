﻿// <auto-generated />
using System;
using GradeLog.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GradeLog.API.Migrations
{
    [DbContext(typeof(GradeLogDbContext))]
    partial class GradeLogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GradeLog.API.Models.Discipline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Disciplines");
                });

            modelBuilder.Entity("GradeLog.API.Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int?>("DisciplineId");

                    b.Property<int>("Mark");

                    b.Property<int?>("StudentId");

                    b.Property<int?>("TeacherId");

                    b.HasKey("Id");

                    b.HasIndex("DisciplineId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("GradeLog.API.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("GradeLog.API.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("GradeLog.API.Models.TeachersDisciplines", b =>
                {
                    b.Property<int>("Discipline_Id");

                    b.Property<int>("Teacher_Id");

                    b.Property<int?>("DisciplineId");

                    b.Property<int?>("TeacherId");

                    b.HasKey("Discipline_Id", "Teacher_Id");

                    b.HasIndex("DisciplineId");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeachersDisciplines");
                });

            modelBuilder.Entity("GradeLog.API.Models.Discipline", b =>
                {
                    b.HasOne("GradeLog.API.Models.Student")
                        .WithMany("Disciplines")
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("GradeLog.API.Models.Grade", b =>
                {
                    b.HasOne("GradeLog.API.Models.Discipline", "Discipline")
                        .WithMany()
                        .HasForeignKey("DisciplineId");

                    b.HasOne("GradeLog.API.Models.Student", "Student")
                        .WithMany("Grades")
                        .HasForeignKey("StudentId");

                    b.HasOne("GradeLog.API.Models.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("GradeLog.API.Models.TeachersDisciplines", b =>
                {
                    b.HasOne("GradeLog.API.Models.Discipline", "Discipline")
                        .WithMany("Teachers")
                        .HasForeignKey("DisciplineId");

                    b.HasOne("GradeLog.API.Models.Teacher", "Teacher")
                        .WithMany("Disciplines")
                        .HasForeignKey("TeacherId");
                });
#pragma warning restore 612, 618
        }
    }
}