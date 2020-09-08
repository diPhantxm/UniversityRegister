using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.DAL.Models
{
    public class Discipline : EntityAbstract<Discipline>
    {
        public string Name { get; set; }
        public ICollection<TeachersDisciplines> Teachers { get; set; }

        public Discipline()
        {

        }

        public Discipline(string name)
        {
            Name = name;
            Teachers = new List<TeachersDisciplines>();
        }
    }
}
