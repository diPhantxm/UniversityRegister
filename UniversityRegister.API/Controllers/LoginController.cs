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

namespace UniversityRegister.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly UniversityRegisterDbContext _context;

        public LoginController(UniversityRegisterDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
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

            var teacherCred = _context.Teachers.Where(t => t.LastName + t.FirstName + t.MiddleName == username).Single();
            var encodedPass = GetHash(password, teacherCred.Salt, teacherCred.Iterations);

            if (encodedPass == teacherCred.Password)
            {
                teacher = teacherCred;
            }

            return teacher;
        }

        private byte[] GenerateSalt(int length)
        {
            var bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }
        private string GetHash(string password, byte[] salt, int iterations)
        {
            string hash = String.Empty;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                hash = Convert.ToBase64String(deriveBytes.GetBytes(salt.Length));
            }

            return hash;
        }
        private string GenerateJWT(Teacher teacher)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var username = teacher.LastName + teacher.FirstName + teacher.MiddleName;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("Role", teacher.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}