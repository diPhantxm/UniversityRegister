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
            return await _context.Class.ToListAsync();
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var @class = await _context.Class.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return @class;
        }

        // GET: api/Classes/ByDiscipline/1
        [HttpGet]
        [Route("ByDiscipline/{id:int}")]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByDiscipline(int discipline_Id)
        {
            try
            {
                return await _context.Class
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
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByDiscipline(string discipline)
        {
            try
            {
                return await _context.Class
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

        // POST: api/Classes
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class @class)
        {
            _context.Class.Add(@class);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = @class.Id }, @class);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Class>> DeleteClass(int id)
        {
            var @class = await _context.Class.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            _context.Class.Remove(@class);
            await _context.SaveChangesAsync();

            return @class;
        }

        private bool ClassExists(int id)
        {
            return _context.Class.Any(e => e.Id == id);
        }
    }
}
