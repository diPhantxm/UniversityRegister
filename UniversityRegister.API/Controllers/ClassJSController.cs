using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityRegister.API.Models;

namespace UniversityRegister.API.Controllers
{
    [Route("api/classJS")]
    [ApiController]
    public class ClassJSController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public ActionResult<ClassJS> Get(int id)
        {
            return NotFound();
        }

        // POST: api/ClassJS
        [HttpPost]
        public void Post(ClassJS value)
        {
            var t = 0;
        }
    }
}
