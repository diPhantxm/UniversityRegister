using UniversityRegister.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace UniversityRegister.API.Models
{
    public class Student : IPerson
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentsClasses> StudentsClasses { get; set; }

        public Student()
        {
            Grades = new List<Grade>();
            StudentsClasses = new List<StudentsClasses>();
        }

        public Student(string Id, string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            Grades = new List<Grade>();
            StudentsClasses = new List<StudentsClasses>();
        }

        public Student(string Id, string firstName, string lastName, string middleName, Group group, ICollection<Grade> grades, ICollection<StudentsClasses> studentsClasses)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Group = group;
            Grades = grades;
            StudentsClasses = studentsClasses;
        }
    }
}
