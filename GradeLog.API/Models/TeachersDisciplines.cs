using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.API.Models
{
    public class TeachersDisciplines
    {
        public int Teacher_Id { get; set; }
        public int Discipline_Id { get; set; }
        public Teacher Teacher { get; set; }
        public Discipline Discipline { get; set; }

        public TeachersDisciplines()
        {

        }

        public TeachersDisciplines(Teacher teacher, Discipline discipline)
        {
            Teacher = teacher;
            Discipline = discipline;
            Teacher_Id = Teacher.Id;
            Discipline_Id = Discipline.Id;
        }
    }
}
