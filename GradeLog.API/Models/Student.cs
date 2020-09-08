using GradeLog.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.API.Models
{
    public class Student : EntityAbstract<Student>, IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Discipline> Disciplines { get; set; }

        public Student()
        {

        }

        public Student(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            Grades = new List<Grade>();
            Disciplines = new List<Discipline>();
        }

        public Student(string firstName, string lastName, string middleName, ICollection<Grade> grades, ICollection<Discipline> disciplines)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Grades = grades;
            Disciplines = disciplines;
        }
    }
}
