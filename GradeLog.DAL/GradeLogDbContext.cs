using GradeLog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace GradeLog.DAL
{
    public class GradeLogDbContext : DbContext
    {
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeachersDisciplines> TeachersDisciplines { get; set; }

        public GradeLogDbContext() : base()
        {
            
        }

        public GradeLogDbContext(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeachersDisciplines>()
                .HasKey(td => new { td.Discipline_Id, td.Teacher_Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
