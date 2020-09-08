using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.DAL.Models
{
    public class Grade : EntityAbstract<Grade>
    {
        public int Mark { get; set; }
        public DateTime Date { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
