using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradeLog.API;
using GradeLog.API.Models;

namespace GradeLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private readonly GradeLogDbContext _context;

        public DisciplinesController(GradeLogDbContext context)
        {
            _context = context;
        }

        // GET: api/Disciplines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplines()
        {
            return await _context.Disciplines.ToListAsync();
        }

        // GET: api/Disciplines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Discipline>> GetDiscipline(int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);

            if (discipline == null)
            {
                return NotFound();
            }

            return discipline;
        }

        // PUT: api/Disciplines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscipline(int id, Discipline discipline)
        {
            if (id != discipline.Id)
            {
                return BadRequest();
            }

            _context.Entry(discipline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisciplineExists(id))
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

        // POST: api/Disciplines
        [HttpPost]
        public async Task<ActionResult<Discipline>> PostDiscipline(Discipline discipline)
        {
            _context.Disciplines.Add(discipline);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiscipline", new { id = discipline.Id }, discipline);
        }

        // DELETE: api/Disciplines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Discipline>> DeleteDiscipline(int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null)
            {
                return NotFound();
            }

            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();

            return discipline;
        }

        private bool DisciplineExists(int id)
        {
            return _context.Disciplines.Any(e => e.Id == id);
        }
    }
}
