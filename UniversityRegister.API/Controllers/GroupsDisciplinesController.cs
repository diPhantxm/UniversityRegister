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
    public class GroupsDisciplinesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public GroupsDisciplinesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/GroupsDisciplines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupsDisciplines>>> GetGroupsDisciplines()
        {
            return await _context.GroupsDisciplines.ToListAsync();
        }

        // GET: api/GroupsDisciplines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupsDisciplines>> GetGroupsDisciplines(int id)
        {
            var groupsDisciplines = await _context.GroupsDisciplines.FindAsync(id);

            if (groupsDisciplines == null)
            {
                return NotFound();
            }

            return groupsDisciplines;
        }

        // PUT: api/GroupsDisciplines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupsDisciplines(int id, GroupsDisciplines groupsDisciplines)
        {
            _context.Entry(groupsDisciplines).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupsDisciplinesExists(id))
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

        // POST: api/GroupsDisciplines
        [HttpPost]
        public async Task<ActionResult<GroupsDisciplines>> PostGroupsDisciplines(GroupsDisciplines groupsDisciplines)
        {
            _context.GroupsDisciplines.Add(groupsDisciplines);
            var group = _context.Groups.Single(t => t.Id == groupsDisciplines.Group.Id);
            var discipline = _context.Disciplines.Single(d => d.Id == groupsDisciplines.Discipline.Id);

            _context.Entry(group).State = EntityState.Modified;
            _context.Entry(discipline).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return groupsDisciplines;
        }

        // DELETE: api/GroupsDisciplines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupsDisciplines>> DeleteGroupsDisciplines(int id)
        {
            var groupsDisciplines = await _context.GroupsDisciplines.FindAsync(id);
            if (groupsDisciplines == null)
            {
                return NotFound();
            }

            _context.GroupsDisciplines.Remove(groupsDisciplines);
            await _context.SaveChangesAsync();

            return groupsDisciplines;
        }

        private bool GroupsDisciplinesExists(int id)
        {
            return _context.GroupsDisciplines.Any(gd => gd.DisciplineId == id);
        }
    }
}
