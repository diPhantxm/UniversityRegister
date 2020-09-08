using GradeLog.DAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.DAL.Models
{
    public class Student : EntityAbstract<Student>, IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Discipline> Disciplines { get; set; }
    }
}
