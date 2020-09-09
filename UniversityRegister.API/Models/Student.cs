using UniversityRegister.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.API.Models
{
    public class Student : EntityAbstract<Student>, IPerson
    {
        public new string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }

        public Student()
        {

        }

        public Student(string Id, string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            Grades = new List<Grade>();
        }

        public Student(string Id, string firstName, string lastName, string middleName, Group group, ICollection<Grade> grades)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Group = group;
            Grades = grades;
        }
    }
}
