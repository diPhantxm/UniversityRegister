using UniversityRegister.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.API.Models
{
    public class Teacher : EntityAbstract<Teacher>, IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }

        public ICollection<TeachersDisciplines> Disciplines { get; set; }
        public ICollection<Grade> Grades { get; set; }

        public Teacher()
        {

        }

        public Teacher(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            Disciplines = new List<TeachersDisciplines>();
            Grades = new List<Grade>();
        }

        public Teacher(string firstName, string lastName, string middleName, ICollection<TeachersDisciplines> disciplines, ICollection<Grade> grades)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Disciplines = disciplines;
            Grades = grades;
        }
    }
}
