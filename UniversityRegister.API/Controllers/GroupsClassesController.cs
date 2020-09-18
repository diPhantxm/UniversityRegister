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
    public class GroupsClassesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public GroupsClassesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/GroupsClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupsClasses>>> GetGroupsClasses()
        {
            return await _context.GroupsClasses.ToListAsync();
        }

        // GET: api/GroupsClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupsClasses>> GetGroupsClasses(int id)
        {
            var groupsClasses = await _context.GroupsClasses.FindAsync(id);

            if (groupsClasses == null)
            {
                return NotFound();
            }

            return groupsClasses;
        }

        // PUT: api/GroupsClasses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupsClasses(int id, GroupsClasses groupsClasses)
        {
            _context.Entry(groupsClasses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupsClassesExists(id))
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

        // POST: api/GroupsClasses
        [HttpPost]
        public async Task<ActionResult<GroupsClasses>> PostGroupsClasses(GroupsClasses groupsClasses)
        {
            var group = _context.Groups.Single(g => g.Id == groupsClasses.Group.Id);
            var @class = _context.Classes.Single(c => c.Id == groupsClasses.Class.Id);

            _context.GroupsClasses.Add(groupsClasses);

            _context.Entry(group).State = EntityState.Modified;
            _context.Entry(@class).State = EntityState.Modified;

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

        // DELETE: api/GroupsClasses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupsClasses>> DeleteGroupsClasses(int id)
        {
            var groupsClasses = await _context.GroupsClasses.FindAsync(id);
            if (groupsClasses == null)
            {
                return NotFound();
            }

            _context.GroupsClasses.Remove(groupsClasses);
            await _context.SaveChangesAsync();

            return groupsClasses;
        }

        private bool GroupsClassesExists(int id)
        {
            return _context.GroupsClasses.Any(e => e.ClassId == id);
        }
    }
}
