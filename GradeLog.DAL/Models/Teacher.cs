using GradeLog.DAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.DAL.Models
{
    public class Teacher : EntityAbstract<Teacher>, IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public ICollection<TeachersDisciplines> Disciplines { get; set; }
    }
}
