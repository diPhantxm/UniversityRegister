using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniversityRegister.API;
using UniversityRegister.API.Models;

namespace UniversityRegister.API.Controllers
{
    [Authorize]
    [Route("api/classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public ClassesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClass()
        {
            return await _context.Classes.ToListAsync();
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var @class = await _context.Classes.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return @class;
        }

        // GET: api/Classes/ByDiscipline/1
        [HttpGet]
        [Route("ByDiscipline/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByDiscipline(int discipline_Id)
        {
            try
            {
                return await _context.Classes
                    .Where(c => c.Discipline.Id == discipline_Id)
                    .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Classes/ByDiscipline/math
        [HttpGet]
        [Route("ByDiscipline/{discipline}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByDiscipline(string discipline)
        {
            try
            {
                return await _context.Classes
                .Where(c => c.Discipline.Name == discipline)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {

                throw;
            }
        }

        // GET: api/Classes/ByGroup/2
        [HttpGet]
        [Route("ByGroup/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByGroup(int group_Id)
        {
            try
            {
                return await _context.GroupsClasses
                .Where(gc => gc.Group.Id == group_Id)
                .Select(gc => gc.Class)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            
        }

        [HttpGet]
        [Route("ByGroupDiscipline/{group_Id:int}/{discipline_Id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByGroupDiscipline(int group_Id, int discipline_Id)
        {
            try
            {
                return await _context.GroupsClasses
                    .Where(gc => gc.Group.Id == group_Id && gc.Class.Discipline.Id == discipline_Id)
                    .Select(gc => gc.Class)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // PUT: api/Classes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, Class @class)
        {
            if (id != @class.Id)
            {
                return BadRequest();
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Classes/Get
        [HttpPost]
        [Route("Get")]
        public async Task<ActionResult<Class>> PostClass(Class @class)
        {
            _context.Classes.Add(@class);
            await new DisciplinesController(_context).PutDiscipline(@class.Discipline.Id, @class.Discipline);
            if (@class.StudentsClasses != null && @class.StudentsClasses.Count > 0)
            {
                foreach (var student in @class.StudentsClasses)
                {
                    _context.Entry(student).State = EntityState.Added;
                }
            }
            if (@class.Groups != null && @class.Groups.Count > 0)
            {
                foreach (var group in @class.Groups)
                {
                    _context.Entry(group).State = EntityState.Added;
                }
            }

            await _context.SaveChangesAsync();

            return await GetClass(@class.Id);
        }

        // POST: api/Classes/Add
        [HttpPost]
        [Route("Add")]
        [AllowAnonymous]
        public async Task<ActionResult<Class>> AddClasses(ClassJS classJson)
        {
            var @class = classJson;

            try
            {
                var discipline = _context.Disciplines.Single(d => d.Id == @class.DisciplineId);
                var group = _context.Groups.Single(g => g.Id == @class.GroupId);
                var students = new List<Student>();

                foreach (var student in @class.VisitedStudents)
                {
                    students.Add(_context.Students.Single(s => s.Id == student));
                }

                var dateSplit = classJson.Date.Split(".");
                var year = int.Parse(dateSplit[2]);
                var month = int.Parse(dateSplit[1]);
                var day = int.Parse(dateSplit[0]);
                var newClass = new Class(new DateTime(year, month, day), null, discipline, null);
                var addResponse = await PostClass(newClass);
                
                var addedClass = addResponse.Value;

                var gc = new GroupsClasses(group, addedClass);
                await new GroupsClassesController(_context).PostGroupsClasses(gc);

                foreach (var student in students)
                {
                    await new StudentsClassesController(_context).PostStudentsClasses(new StudentsClasses(student, addedClass));
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Class>> DeleteClass(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();

            return @class;
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
