using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniversityRegister.API;
using UniversityRegister.API.Models;
using UniversityRegister.API.Models.Security;

namespace UniversityRegister.API.Controllers
{
    [Route("api/Teachers")]
    [ApiController]
    [Authorize]
    public class TeachersController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public TeachersController(UniversityRegisterDbContext context, IConfiguration _config)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.Select(t => (Teacher)t).ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // GET: api/Teachers/ByDiscipline/5
        [HttpGet]
        [Route("ByDyscipline/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachersByDiscipline(int discipline_Id)
        {
            try
            {
                var result = await _context.TeachersDisciplines
                .Where(td => td.Discipline.Id == discipline_Id)
                .Select(td => td.Teacher)
                .ToListAsync();
                return result;
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // POST: api/Teachers/Add
        [HttpPost("Add")]
        public async Task<ActionResult<Teacher>> PostTeacher(TeacherCred teacher)
        {
            var rnd = new Random();
            var salt = SecurityManager.GenerateSalt(rnd.Next() % 12 + 8);
            var iterations = rnd.Next() % 12 + 1;
            var hash = SecurityManager.GetHash(teacher.Password, salt, iterations);

            teacher.Salt = salt;
            teacher.Iterations = iterations;
            teacher.Password = hash;

            _context.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacher", new { id = teacher.Id }, teacher);
        }

        // POST: api/Teacher/Get
        [HttpPost("Get")]
        [AllowAnonymous]
        public async Task<ActionResult<Teacher>> PostGetTeacher(Teacher teacher)
        {
            try
            {
                var teacherFind = await _context.Teachers.SingleAsync(t => 
                    t.FirstName == teacher.FirstName &&
                    t.LastName == teacher.LastName &&
                    t.MiddleName == teacher.MiddleName);

                return teacher;
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        // PUT: api/Teachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Teacher>> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return teacher;
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
