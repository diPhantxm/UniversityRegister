using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class Class : EntityAbstract<Class>
    {
        public DateTime Date { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual ICollection<GroupsClasses> Groups { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentsClasses> StudentsClasses { get; set; }

        public Class()
        {

        }

        public Class(DateTime date)
        {
            Date = date;
            Groups = new List<GroupsClasses>();
            StudentsClasses = new List<StudentsClasses>();
        }

        public Class(DateTime date, ICollection<GroupsClasses> groups, Discipline discipline, ICollection<StudentsClasses> studentsClasses)
        {
            Date = date;
            Groups = groups;
            Discipline = discipline;
            StudentsClasses = studentsClasses;
        }
    }
}
