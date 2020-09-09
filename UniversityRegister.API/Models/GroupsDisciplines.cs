using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class GroupsDisciplines : EntityAbstract<GroupsDisciplines>
    {
        public virtual Group Group { get; set; }
        public virtual Discipline Discipline { get; set; }

        public GroupsDisciplines()
        {

        }
    }
}
