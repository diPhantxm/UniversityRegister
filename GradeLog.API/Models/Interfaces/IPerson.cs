using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.API.Models.Interfaces
{
    interface IPerson
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
    }
}
