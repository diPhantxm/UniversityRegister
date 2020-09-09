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
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public GradesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/Grades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        // GET: api/Grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        // GET: api/Grades/ByStudent/5
        [HttpGet]
        [Route("ByStudent/{id:int}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByStudent(string student_Id)
        {
            try
            {
                return await _context.Grades
                .Where(g => g.Student.Id == student_Id)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Grades/ByTeacher/5
        [HttpGet]
        [Route("ByTeacher/{id:int}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByTeacher(int teacher_Id)
        {
            try
            {
                return await _context.Grades
                .Where(g => g.Teacher.Id == teacher_Id)
                .ToListAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
 
        }

        // GET: api/Grades/ByTeacher/Ivanov/Ivan/Ivanovich
        [HttpGet]
        [Route("ByTeacher/{firstName}/{middleName}/{lastName}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByTeacher(string firstName, string middleName, string lastName)
        {
            try
            {
                return await _context.Grades
                .Where(g => g.Teacher.FirstName == firstName &&
                        g.Teacher.MiddleName == g.Teacher.MiddleName &&
                        g.Teacher.LastName == lastName)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Grades/ByDiscipline/5
        [HttpGet]
        [Route("ByDiscipline/{id:int}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByDiscipline(int id)
        {
            try
            {
                return await _context.Grades
                .Where(g => g.Discipline.Id == id)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Grades/ByDiscipline/math
        [HttpGet]
        [Route("ByDiscipline/{name}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByDiscipline(string discipline)
        {
            try
            {
                return await _context.Grades
                    .Where(g => g.Discipline.Name == discipline)
                    .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // PUT: api/Grades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int id, Grade grade)
        {
            if (id != grade.Id)
            {
                return BadRequest();
            }

            _context.Entry(grade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
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

        // POST: api/Grades
        [HttpPost]
        public async Task<ActionResult<Grade>> PostGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrade", new { id = grade.Id }, grade);
        }

        // DELETE: api/Grades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Grade>> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();

            return grade;
        }

        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }
    }
}
