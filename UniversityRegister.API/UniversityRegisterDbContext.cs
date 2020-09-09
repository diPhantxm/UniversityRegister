using UniversityRegister.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace UniversityRegister.API
{
    public class UniversityRegisterDbContext : DbContext
    {
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherCred> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<TeachersDisciplines> TeachersDisciplines { get; set; }
        public DbSet<GroupsDisciplines> GroupsDisciplines { get; set; }
        public DbSet<GroupsClasses> GroupsClasses { get; set; }

        public UniversityRegisterDbContext()
        {

        }

        public UniversityRegisterDbContext(DbContextOptions<UniversityRegisterDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TeachersDisciplines>()
            //    .HasKey(td => new { td.Discipline_Id, td.Teacher_Id });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UniversityRegister.API.Models.Class> Class { get; set; }

        public DbSet<UniversityRegister.API.Models.Group> Group { get; set; }
    }
}
