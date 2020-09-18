using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UniversityRegister.API.Models
{
    public class TeachersDisciplines : EntityAbstract<TeachersDisciplines>
    {
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        [ForeignKey("Discipline")]
        public int DisciplineId { get; set; }

        public TeacherCred Teacher { get; set; }
        public Discipline Discipline { get; set; }

        public TeachersDisciplines()
        {

        }

        public TeachersDisciplines(TeacherCred teacher, Discipline discipline)
        {
            Teacher = teacher;
            Discipline = discipline;
        }
    }
}
