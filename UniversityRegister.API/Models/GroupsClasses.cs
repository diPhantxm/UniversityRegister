using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class GroupsClasses : EntityAbstract<GroupsClasses>
    {
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public Group Group { get; set; }
        public Class Class { get; set; } 

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
