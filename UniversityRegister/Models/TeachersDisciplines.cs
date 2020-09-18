using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.Models
{
    public class TeachersDisciplines : EntityAbstract<TeachersDisciplines>
    {
        public Teacher Teacher { get; set; }
        public Discipline Discipline { get; set; }

        public TeachersDisciplines()
        {

        }

        public TeachersDisciplines(Teacher teacher, Discipline discipline)
        {
            Teacher = teacher;
            Discipline = discipline;
        }
    }
}
