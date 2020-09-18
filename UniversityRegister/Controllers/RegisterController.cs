using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using UniversityRegister.Models;
using Microsoft.AspNetCore.Hosting;
using UniversityRegister.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace UniversityRegister.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class RegisterController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UniAPI _uniAPI;
        private readonly string _rootPath;

        public RegisterController(IConfiguration config, UniAPI uniAPI)
        {
            _config = config;
            _uniAPI = uniAPI;
            _rootPath = _config.GetValue<string>(WebHostDefaults.ContentRootKey);
        }

        // GET: Register
        public async Task<ActionResult> Index()
        {
            var token = new Jwt()
            {
                Token = HttpContext.Request.Cookies["Jwt"]
            };
            _uniAPI.SetJWT(token);

            ViewBag.Disciplines = new List<SelectListItem>();
            ViewBag.Groups = new List<SelectListItem>();
            ViewBag.Classes = new List<SelectListItem>();

            var teacher_Id = User.Claims.Where(c => c.Type == "Id").Single().Value;

            var disciplinesResponse = await _uniAPI.Get<IEnumerable<Discipline>>($"disciplines/ByTeacher/{ teacher_Id }");
            if (disciplinesResponse.Value != null)
            {
                ViewBag.Disciplines = disciplinesResponse.Value.Select(d => new SelectListItem(d.Name, d.Id.ToString()));
            }

            return View();
        } 
    }
}