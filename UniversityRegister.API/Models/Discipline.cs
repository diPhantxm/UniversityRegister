using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace UniversityRegister.API.Models
{
    public class Discipline : EntityAbstract<Discipline>
    {
        public string Name { get; set; }
        public ICollection<Class> Classes { get; set; }
        [JsonIgnore]
        public ICollection<GroupsDisciplines> GroupsDisciplines { get; set; }
        [JsonIgnore]
        public ICollection<TeachersDisciplines> Teachers { get; set; }

        public Discipline()
        {
            Classes = new List<Class>();
            GroupsDisciplines = new List<GroupsDisciplines>();
            Teachers = new List<TeachersDisciplines>();
        }

        public Discipline(string name)
        {
            Name = name;
            Teachers = new List<TeachersDisciplines>();
            Classes = new List<Class>();
            GroupsDisciplines = new List<GroupsDisciplines>();
        }

        public Discipline(string name, ICollection<TeachersDisciplines> teachers, ICollection<Class> classes, ICollection<GroupsDisciplines> groupsDisciplines)
        {
            Name = name;
            Teachers = teachers;
            Classes = classes;
            GroupsDisciplines = groupsDisciplines;
        }
    }
}
