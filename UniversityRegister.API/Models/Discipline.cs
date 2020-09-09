using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.API.Models
{
    public class Discipline : EntityAbstract<Discipline>
    {
        public string Name { get; set; }
        public ICollection<Class> StudentsClasses { get; set; }
        public ICollection<GroupsDisciplines> GroupsDisciplines { get; set; }
        public ICollection<TeachersDisciplines> Teachers { get; set; }

        public Discipline()
        {

        }

        public Discipline(string name)
        {
            Name = name;
            Teachers = new List<TeachersDisciplines>();
        }

        public Discipline(string name, ICollection<TeachersDisciplines> teachers)
        {
            Name = name;
            Teachers = teachers;
        }
    }
}
