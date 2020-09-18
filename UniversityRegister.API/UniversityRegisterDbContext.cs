using UniversityRegister.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.SqlServer;
using System.Linq;
using System;
using UniversityRegister.API.Controllers;

namespace UniversityRegister.API
{
    public class UniversityRegisterDbContext : DbContext
    {
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherCred> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<TeachersDisciplines> TeachersDisciplines { get; set; }
        public DbSet<GroupsDisciplines> GroupsDisciplines { get; set; }
        public DbSet<GroupsClasses> GroupsClasses { get; set; }
        public DbSet<StudentsClasses> StudentsClasses { get; set; }

        public UniversityRegisterDbContext()
        {
            
        }

        public UniversityRegisterDbContext(DbContextOptions<UniversityRegisterDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationships between Teachers and Disciplines
            modelBuilder.Entity<TeachersDisciplines>()
                .HasKey(td => new { td.DisciplineId, td.TeacherId });

            modelBuilder.Entity<TeachersDisciplines>()
                .HasOne(td => td.Teacher)
                .WithMany(t => t.Disciplines)
                .HasForeignKey(td => td.TeacherId)
                .IsRequired();

            modelBuilder.Entity<TeachersDisciplines>()
                .HasOne(td => td.Discipline)
                .WithMany(d => d.Teachers)
                .HasForeignKey(td => td.DisciplineId)
                .IsRequired();

            // Configure many-to-many relationships between Groups and Disciplines
            modelBuilder.Entity<GroupsDisciplines>()
                .HasKey(td => new { td.DisciplineId, td.GroupId });

            modelBuilder.Entity<GroupsDisciplines>()
                .HasOne(td => td.Group)
                .WithMany(t => t.Disciplines)
                .HasForeignKey(td => td.GroupId)
                .IsRequired();

            modelBuilder.Entity<GroupsDisciplines>()
                .HasOne(td => td.Discipline)
                .WithMany(d => d.GroupsDisciplines)
                .HasForeignKey(td => td.DisciplineId)
                .IsRequired();

            // Configure many-to-many relationships between Teachers and Disciplines
            modelBuilder.Entity<TeachersDisciplines>()
                .HasKey(td => new { td.DisciplineId, td.TeacherId });

            modelBuilder.Entity<TeachersDisciplines>()
                .HasOne(td => td.Teacher)
                .WithMany(t => t.Disciplines)
                .HasForeignKey(td => td.TeacherId)
                .IsRequired();

            modelBuilder.Entity<TeachersDisciplines>()
                .HasOne(td => td.Discipline)
                .WithMany(d => d.Teachers)
                .HasForeignKey(td => td.DisciplineId)
                .IsRequired();

            // Configure many-to-many relationships between Students and Classes
            modelBuilder.Entity<StudentsClasses>()
                .HasKey(td => new { td.StudentId, td.ClassId });

            modelBuilder.Entity<StudentsClasses>()
                .HasOne(sc => sc.Class)
                .WithMany(c => c.StudentsClasses)
                .HasForeignKey(sc => sc.ClassId)
                .IsRequired();

            modelBuilder.Entity<StudentsClasses>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentsClasses)
                .HasForeignKey(sc => sc.StudentId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
