using UniversityRegister.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.Models
{
    public class Student : EntityAbstract<Student>, IPerson
    {
        public new string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<StudentsClasses> Classes { get; set; }

        public Student()
        {

        }

        public Student(string id, string firstName, string lastName, string middleName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            Grades = new List<Grade>();
            Classes = new List<StudentsClasses>();
        }

        public Student(string id, string firstName, string lastName, string middleName, Group group, ICollection<Grade> grades, ICollection<StudentsClasses> classes)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            Group = group;
            Grades = grades;
            Classes = classes;
        }
    }
}
