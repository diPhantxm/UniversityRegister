using UniversityRegister.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.Models
{
    public class Student : EntityAbstract<Student>
    {
        public new string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<StudentsClasses> Classes { get; set; }

        public Student()
        {

        }

        public Student(string id, string firstName, string lastName, string mail)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Mail = mail;

            Grades = new List<Grade>();
            Classes = new List<StudentsClasses>();
        }

        public Student(string id, string firstName, string lastName, string mail, Group group, ICollection<Grade> grades, ICollection<StudentsClasses> classes)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Mail = mail;

            Group = group;
            Grades = grades;
            Classes = classes;
        }
    }
}
