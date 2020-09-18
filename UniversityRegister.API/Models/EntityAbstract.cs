using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityRegister.API.Models
{
    public abstract class EntityAbstract<T> where T : class
    {
        public int Id { get; set; }
    }
}
