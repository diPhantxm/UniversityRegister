using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UniversityRegister.API.Models;
using UniversityRegister.API.Models.Security;

namespace UniversityRegister.API.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UniversityRegisterDbContext _context;

        public LoginController(IConfiguration config, UniversityRegisterDbContext context)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult Login(Credentials creds)
        {
            var username = creds.Username;
            var password = creds.Password;

            IActionResult response = Unauthorized();

            var teacher = Authenticate(username, password);

            if (teacher != null)
            {
                var tokenStr = GenerateJWT(teacher);
                response = Ok(new { token = tokenStr });
            }

            return response;
        }

        private Teacher Authenticate(string username, string password)
        {
            Teacher teacher = null;

            try
            {
                var teacherCred = _context.Teachers.Where(t => String.Format($"{ t.LastName } { t.FirstName } { t.MiddleName }") == username).Single();
                var encodedPass = SecurityManager.GetHash(password, teacherCred.Salt, teacherCred.Iterations);

                if (encodedPass == teacherCred.Password)
                {
                    teacher = teacherCred;
                }

                return teacher;
            }
            catch (Exception)
            {
                return teacher;
            }
        }

        private string GenerateJWT(Teacher teacher)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var username = teacher.LastName + teacher.FirstName + teacher.MiddleName;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, teacher.Role),
                new Claim("Id", teacher.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}