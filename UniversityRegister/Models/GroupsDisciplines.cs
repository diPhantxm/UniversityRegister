using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class GroupsDisciplines : EntityAbstract<GroupsDisciplines>
    {
        public virtual Group Group { get; set; }
        public virtual Discipline Discipline { get; set; }

        public GroupsDisciplines()
        {

        }


        public GroupsDisciplines(Group group, Discipline discipline)
        {
            Group = group;
            Discipline = discipline;
        }
    }
}
