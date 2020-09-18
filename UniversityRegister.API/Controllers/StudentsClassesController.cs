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
    public class StudentsClassesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public StudentsClassesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentsClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentsClasses>>> GetStudentsClasses()
        {
            return await _context.StudentsClasses.ToListAsync();
        }

        // GET: api/StudentsClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentsClasses>> GetStudentsClasses(int id)
        {
            var studentsClasses = await _context.StudentsClasses.FindAsync(id);

            if (studentsClasses == null)
            {
                return NotFound();
            }

            return studentsClasses;
        }

        // PUT: api/StudentsClasses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentsClasses(int id, StudentsClasses studentsClasses)
        {
            if (id != studentsClasses.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentsClasses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsClassesExists(id))
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

        // POST: api/StudentsClasses
        [HttpPost]
        public async Task<ActionResult<StudentsClasses>> PostStudentsClasses(StudentsClasses studentsClasses)
        {
            try
            {
                var student = _context.Students.Single(s => s.Id == studentsClasses.Student.Id);
                var @class = _context.Classes.Single(c => c.Id == studentsClasses.Class.Id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            _context.StudentsClasses.Add(studentsClasses);

            _context.Entry(studentsClasses.Student).State = EntityState.Modified;
            _context.Entry(studentsClasses.Class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/StudentsClasses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StudentsClasses>> DeleteStudentsClasses(int id)
        {
            var studentsClasses = await _context.StudentsClasses.FindAsync(id);
            if (studentsClasses == null)
            {
                return NotFound();
            }

            _context.StudentsClasses.Remove(studentsClasses);
            await _context.SaveChangesAsync();

            return studentsClasses;
        }

        private bool StudentsClassesExists(int id)
        {
            return _context.StudentsClasses.Any(e => e.Id == id);
        }
    }
}
