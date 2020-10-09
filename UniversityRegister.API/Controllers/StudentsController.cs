using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityRegister.API;
using UniversityRegister.API.Models;

namespace UniversityRegister.API.Controllers
{
    [Authorize]
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public StudentsController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.OrderBy(s => s.LastName).ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Student>> GetStudent(string id)
        {
            return await Task.Run<ActionResult<Student>>(() =>
            {
                try
                {
                    var student = _context.Students.Where(s => s.Id == id).Single();

                    if (student == null)
                    {
                        return NotFound();
                    }

                    return student;
                }
                catch (Exception)
                {
                    return NotFound();
                }
            });
        }

        // GET: api/Students/ByClass/5
        [HttpGet]
        [Route("ByClass/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByClass(int class_Id)
        {
            try
            {
                return await _context.StudentsClasses
                .Where(sc => sc.Class.Id == class_Id)
                .Select(sc => sc.Student)
                .OrderBy(s => s.LastName)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // POST: api/Students/ByClasses
        [HttpPost]
        [Route("ByClasses")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<IEnumerable<Student>>>> GetStudentsByClasses(IEnumerable<int> classIds)
        {
            try
            {
                var students = new List<IEnumerable<Student>>();
                foreach (var classId in classIds)
                {
                    students.Add((await GetStudentsByClass(classId)).Value);
                }
                return students;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("ByGroup/{group_Id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByGroup(int group_Id)
        {
            try
            {
                return await _context.Students
                    .Where(s => s.Group.Id == group_Id)
                    .OrderBy(s => s.LastName)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            var group = student.Group;
            _context.Entry(group).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return student;
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
