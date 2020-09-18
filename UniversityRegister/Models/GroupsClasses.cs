using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class GroupsClasses : EntityAbstract<GroupsClasses>
    {
        public virtual Group Group { get; set; }
        public virtual Class Class { get; set; }

        public GroupsClasses()
        {

        }

        public GroupsClasses(Group group, Class @class)
        {
            Group = group;
            Class = @class;
        }
    }
}
