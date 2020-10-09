using UniversityRegister.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace UniversityRegister.API.Models
{
    public class Student
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentsClasses> StudentsClasses { get; set; }

        public Student()
        {
            Grades = new List<Grade>();
            StudentsClasses = new List<StudentsClasses>();
        }

        public Student(string id, string firstName, string lastName, string mail)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Mail = mail;

            Grades = new List<Grade>();
            StudentsClasses = new List<StudentsClasses>();
        }

        public Student(string id, string firstName, string lastName, string mail, Group group, ICollection<Grade> grades, ICollection<StudentsClasses> studentsClasses)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Mail = mail;

            Group = group;
            Grades = grades;
            StudentsClasses = studentsClasses;
        }
    }
}
