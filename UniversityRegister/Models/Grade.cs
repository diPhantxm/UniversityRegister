using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.Models
{
    public class Grade : EntityAbstract<Grade>
    {
        public int Mark { get; set; }
        public DateTime Date { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Grade()
        {

        }

        public Grade(int mark, DateTime date, Discipline discipline, Student student, Teacher teacher)
        {
            Mark = mark;
            Date = date;
            Discipline = discipline;
            Student = student;
            Teacher = teacher;
        }
    }
}
