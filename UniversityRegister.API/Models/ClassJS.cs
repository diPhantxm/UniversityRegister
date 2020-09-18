using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.API.Models
{
    public class ClassJS
    {
        public string Date { get; set; }
        public int DisciplineId { get; set; }
        public int GroupId { get; set; }
        public IEnumerable<string> VisitedStudents { get; set; }
    }
}
