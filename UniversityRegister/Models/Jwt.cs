using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityRegister.Models
{
    public class Jwt
    {
        private string _token;

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
            }
        }
    }
}
