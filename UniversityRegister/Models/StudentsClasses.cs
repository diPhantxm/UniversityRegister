using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class StudentsClasses : EntityAbstract<StudentsClasses>
    {
        public Student Student { get; set; }
        public Class Class { get; set; }

        public StudentsClasses()
        {

        }

        public StudentsClasses(Student student, Class @class)
        {
            Student = student;
            Class = @class;
        }
    }
}
