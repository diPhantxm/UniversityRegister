using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class StudentsClasses : EntityAbstract<StudentsClasses>
    {

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }

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
