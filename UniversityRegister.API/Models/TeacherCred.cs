using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class TeacherCred : Teacher
    {
        public byte[] Salt { get; set; }
        public int Iterations { get; set; }
        public string Password { get; set; }

        public TeacherCred(string firstName, string lastName, string middleName, string role, string password, 
            ICollection<TeachersDisciplines> disciplines, ICollection<Grade> grades) : base(firstName, lastName, middleName, disciplines, grades)
        {
            Password = password;
        }

        public TeacherCred() : base()
        {

        }
    }
}
