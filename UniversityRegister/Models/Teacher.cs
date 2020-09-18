using UniversityRegister.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UniversityRegister.API.Models;

namespace UniversityRegister.Models
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

        public Teacher(string firstName, string lastName, string middleName, string role = "Teacher")
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Role = role;

            Disciplines = new List<TeachersDisciplines>();
            Grades = new List<Grade>();
        }

        public Teacher(string firstName, string lastName, string middleName, ICollection<TeachersDisciplines> disciplines, ICollection<Grade> grades, string role = "Teacher")
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Role = role;

            Disciplines = disciplines;
            Grades = grades;
        }
    }
}
