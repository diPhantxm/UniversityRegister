using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityRegister.API;
using UniversityRegister.API.Models;

namespace UniversityRegister.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Groups")]
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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup()
        {
            return await _context.Groups.ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var @group = await _context.Groups.FindAsync(id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        // GET: api/Groups/evm-32
        [HttpGet]
        [Route("{name:alpha}")]
        [AllowAnonymous]
        public async Task<ActionResult<Group>> GetGroup(string name)
        {
            return await Task.Run<ActionResult<Group>>(() =>
            {
                try
                {
                    return _context.Groups.Where(g => g.Name == name).Single();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            });
        }

        // GET: api/Groups/ByDiscipline/math
        [HttpGet]
        [Route("ByDiscipline/{disciplineName:alpha}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByDisciplineName(string disciplineName)
        {
            try
            {
                return await _context.GroupsDisciplines
                .Where(gd => gd.Discipline.Name == disciplineName)
                .Select(gd => gd.Group)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: api/Groups/ByDiscipline/5
        [HttpGet("ByDiscipline/{discipline_Id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByDiscipline(int discipline_Id)
        {
            try
            {
                var result = await _context.GroupsDisciplines
                .Where(gd => gd.Discipline.Id == discipline_Id)
                .Select(gd => gd.Group)
                .ToListAsync();
                return result;
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        // GET: api/Groups/ByClass/5
        [HttpGet]
        [Route("ByClass/{id:int}")]
        [AllowAnonymous]
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

        // POST: api/Groups/Add
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            _context.Groups.Add(@group);
            if (group.Disciplines != null && group.Disciplines.Count > 0)
            {
                foreach (var discipline in group.Disciplines)
                {
                    _context.Entry(discipline).State = EntityState.Added;
                }
            }
            if (group.Classes != null && group.Classes.Count > 0)
            {
                foreach (var @class in group.Classes)
                {
                    _context.Entry(@class).State = EntityState.Added;
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
        }

        // POST: api/Groups/Get
        [HttpPost]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<ActionResult<Group>> GetGroup(Group @group)
        {
            return await Task.Run<ActionResult<Group>>(() =>
            {
                try
                {
                    return Ok(_context.Groups.Where(g => g.Name == @group.Name).Single());
                }
                catch (Exception)
                {
                    return NotFound();
                }
            });
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();

            return @group;
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
