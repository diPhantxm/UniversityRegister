using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class Group : EntityAbstract<Group>
    {
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<GroupsClasses> Classes { get; set; }
        public ICollection<GroupsDisciplines> Disciplines { get; set; }

        public Group()
        {

        }

        public Group(string name)
        {
            Name = name;
            Students = new List<Student>();
            Disciplines = new List<GroupsDisciplines>();
            Classes = new List<GroupsClasses>();
        }

        public Group(string name, ICollection<Student> students, ICollection<GroupsClasses> classes, ICollection<GroupsDisciplines> disciplines)
        {
            Name = name;
            Students = students;
            Disciplines = disciplines;
            Classes = classes;
        }
    }
}
