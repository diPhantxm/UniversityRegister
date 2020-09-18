using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.Models.Interfaces
{
    interface IPerson
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
    }
}
