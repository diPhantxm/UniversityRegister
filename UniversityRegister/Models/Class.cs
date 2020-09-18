using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class Class : EntityAbstract<Class>
    {
        public DateTime Date { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public Class()
        {

        }

        public Class(DateTime date)
        {
            Date = date;
            Groups = new List<Group>();
            Students = new List<Student>();
        }

        public Class(DateTime date, ICollection<Group> groups, Discipline discipline, ICollection<Student> students)
        {
            Date = date;
            Groups = groups;
            Discipline = discipline;
            Students = students;
        }
    }
}
