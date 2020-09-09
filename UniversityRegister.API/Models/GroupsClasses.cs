using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class GroupsClasses : EntityAbstract<GroupsClasses>
    {
        public virtual Group Group { get; set; }
        public virtual Class Class { get; set; }

        public GroupsClasses()
        {

        }
    }
}
