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
    public class GroupsController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public GroupsController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup()
        {
            return await _context.Group.ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var @group = await _context.Group.FindAsync(id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        // GET: api/Groups/ByDiscipline/math
        [HttpGet]
        [Route("ByDiscipline/{name}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByDiscipline(string discipline)
        {
            try
            {
                return await _context.GroupsDisciplines
                .Where(gd => gd.Discipline.Name == discipline)
                .Select(gd => gd.Group)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Groups/ByDiscipline/5
        [HttpGet]
        [Route("ByDiscipline/{id:int}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByDiscipline(int discipline_Id)
        {
            try
            {
                return await _context.GroupsDisciplines
                .Where(gd => gd.Discipline.Id == discipline_Id)
                .Select(gd => gd.Group)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Groups/ByClass/5
        [HttpGet]
        [Route("ByClass/{id:int}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByClass(int class_Id)
        {
            try
            {
                return await _context.GroupsClasses
                .Where(gc => gc.Class.Id == class_Id)
                .Select(gc => gc.Group)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            if (id != @group.Id)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            _context.Group.Add(@group);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int id)
        {
            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();

            return @group;
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }
    }
}
