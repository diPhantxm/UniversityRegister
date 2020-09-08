using System;
using System.Collections.Generic;
using System.Text;

namespace GradeLog.DAL.Models
{
    public abstract class EntityAbstract<T> where T : class
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityAbstract<T>))
            {
                return false;
            }

            if (this.Id == (obj as EntityAbstract<T>).Id)
            {
                return true;
            }

            return false;
        }
    }
}
