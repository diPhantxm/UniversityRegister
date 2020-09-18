using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class GroupsDisciplines : EntityAbstract<GroupsDisciplines>
    {
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [ForeignKey("Discipline")]
        public int DisciplineId { get; set; }

        public Group Group { get; set; }
        public Discipline Discipline { get; set; }

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
