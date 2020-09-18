using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityRegister.API;
using UniversityRegister.API.Models;

namespace UniversityRegister.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersDisciplinesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public TeachersDisciplinesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/TeachersDisciplines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeachersDisciplines>>> GetTeachersDisciplines()
        {
            return await _context.TeachersDisciplines.ToListAsync();
        }

        // GET: api/TeachersDisciplines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeachersDisciplines>> GetTeachersDisciplines(int id)
        {
            var teachersDisciplines = await _context.TeachersDisciplines.FindAsync(id);

            if (teachersDisciplines == null)
            {
                return NotFound();
            }

            return teachersDisciplines;
        }

        // PUT: api/TeachersDisciplines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeachersDisciplines(int id, TeachersDisciplines teachersDisciplines)
        {
            if (id != teachersDisciplines.DisciplineId)
            {
                return BadRequest();
            }

            _context.Entry(teachersDisciplines).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeachersDisciplinesExists(id))
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

        // POST: api/TeachersDisciplines
        [HttpPost]
        public async Task<ActionResult<TeachersDisciplines>> PostTeachersDisciplines(TeachersDisciplines teachersDisciplines)
        {
            var teacher = _context.Teachers.Single(t => t.Id == teachersDisciplines.Teacher.Id);
            var discipline = _context.Disciplines.Single(d => d.Id == teachersDisciplines.Discipline.Id);

            _context.TeachersDisciplines.Add(teachersDisciplines);

            _context.Entry(teacher).State = EntityState.Modified;
            _context.Entry(discipline).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeachersDisciplinesExists(teachersDisciplines.DisciplineId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

             return teachersDisciplines;
        }

        // DELETE: api/TeachersDisciplines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TeachersDisciplines>> DeleteTeachersDisciplines(int id)
        {
            var teachersDisciplines = await _context.TeachersDisciplines.FindAsync(id);
            if (teachersDisciplines == null)
            {
                return NotFound();
            }

            _context.TeachersDisciplines.Remove(teachersDisciplines);
            await _context.SaveChangesAsync();

            return teachersDisciplines;
        }

        private bool TeachersDisciplinesExists(int id)
        {
            return _context.TeachersDisciplines.Any(e => e.DisciplineId == id);
        }
    }
}
