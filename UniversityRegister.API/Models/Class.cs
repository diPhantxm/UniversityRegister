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

        public Class()
        {

        }

        public Class(DateTime date)
        {
            Date = date;
        }

        public Class(DateTime date, ICollection<GroupsClasses> groups)
        {
            Date = date;
            Groups = groups;
        }
    }
}
