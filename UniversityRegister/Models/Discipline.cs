using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.Models
{
    public class Discipline : EntityAbstract<Discipline>
    {
        public string Name { get; set; }
        public ICollection<Class> Classes { get; set; }
        public ICollection<GroupsDisciplines> Groups{ get; set; }
        public ICollection<TeachersDisciplines> Teachers { get; set; }

        public Discipline()
        {

        }

        public Discipline(string name)
        {
            Name = name;
            Classes = new List<Class>();
            Groups = new List<GroupsDisciplines>();
            Teachers = new List<TeachersDisciplines>();
        }

        public Discipline(string name, ICollection<TeachersDisciplines> teachers, ICollection<Class> classes, List<GroupsDisciplines> groups)
        {
            Name = name;
            Teachers = teachers;
            Classes = classes;
            Groups = groups;
        }
    }
}
