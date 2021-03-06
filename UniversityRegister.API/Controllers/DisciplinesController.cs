﻿using System;
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
    [Route("api/disciplines")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private readonly UniversityRegisterDbContext _context;

        public DisciplinesController(UniversityRegisterDbContext context)
        {
            _context = context;
        }

        // GET: api/Disciplines
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplines()
        {
            return await _context.Disciplines.ToListAsync();
        }

        // GET: api/Disciplines/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Discipline>> GetDiscipline(int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);

            if (discipline == null)
            {
                return NotFound();
            }

            return discipline;
        }

        [HttpGet("{name:alpha}")]
        [AllowAnonymous]
        public async Task<ActionResult<Discipline>> GetDiscipline(string name)
        {
            return await Task.Run<ActionResult<Discipline>>(() =>
            {
                try
                {
                    return _context.Disciplines.Where(d => d.Name == name).Single();
                }
                catch (Exception)
                {

                    throw;
                }
            });
            
        }

        // GET: api/Disciplines/ByGroup/5
        [HttpGet]
        [Route("ByGroup/{group_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplinesByGroup(int group_Id)
        {
            try
            {
                return await _context.GroupsDisciplines
                .Where(gd => gd.Group.Id == group_Id)
                .Select(gd => gd.Discipline)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            
        }

        // GET: api/Disciplines/ByTeacher/4
        [HttpGet("ByTeacher/{teacher_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplinesByTeacher(int teacher_Id)
        {
            try
            {
                var result = await _context.TeachersDisciplines
                .Where(td => td.Teacher.Id == teacher_Id)
                .Select(td => td.Discipline)
                .ToListAsync();
                return result;
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            
        }

        // GET: api/Discipline/ByTeacher/Ivanov/Ivan/Ivanovich
        [HttpGet]
        [Route("ByTeacher/{firstName}/{middleName}/{lastName}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplinesByTeacherName(string firstName, string middleName, string lastName)
        {
            try
            {
                return await _context.TeachersDisciplines
                .Where(td => td.Teacher.FirstName == firstName &&
                        td.Teacher.MiddleName == middleName &&
                        td.Teacher.LastName == lastName)
                .Select(td => td.Discipline)
                .ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
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
        [Route("Add")]
        public async Task<ActionResult<Discipline>> PostDiscipline(Discipline discipline)
        {
            _context.Disciplines.Add(discipline);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiscipline", new { id = discipline.Id }, discipline);
        }

        // POST: api/Disciplines/math
        [HttpPost]
        [Route("Get")]
        public async Task<ActionResult<Discipline>> GetDiscipline(Discipline disciplne)
        {
            return await Task.Run<ActionResult<Discipline>>(() =>
            {
                try
                {
                    return _context.Disciplines.Where(d => d.Name == disciplne.Name).Single();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            });
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
