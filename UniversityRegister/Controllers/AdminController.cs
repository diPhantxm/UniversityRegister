using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UniversityRegister.Models;
using UniversityRegister.Services;

namespace UniversityRegister.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UniAPI _uniAPI;
        private readonly string _rootPath;

        public AdminController(IConfiguration config, UniAPI uniAPI)
        {
            _config = config;
            _uniAPI = uniAPI;
            _rootPath = _config.GetValue<string>(WebHostDefaults.ContentRootKey);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddTeachers(IFormFileCollection ExcelFiles)
        {
            var jwt = Request.Cookies["Jwt"];
            _uniAPI.SetJWT(jwt);
            if (jwt == null || String.IsNullOrWhiteSpace(jwt))
            {
                await HttpContext.SignOutAsync("Cookies");
                return LocalRedirectPermanent("~/Home");
            }

            await ExcelService.AddTeachers(ExcelFiles, _rootPath, _uniAPI);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddGroups(IFormFileCollection ExcelFiles)
        {
            var jwt = Request.Cookies["Jwt"];
            _uniAPI.SetJWT(jwt);
            if (jwt == null || String.IsNullOrWhiteSpace(jwt))
            {
                await HttpContext.SignOutAsync("Cookies");
                return LocalRedirectPermanent("~/Home");
            }

            await ExcelService.AddGroups(ExcelFiles, _rootPath, _uniAPI);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddStudents(IFormFileCollection ExcelFiles)
        {
            var jwt = Request.Cookies["Jwt"];
            _uniAPI.SetJWT(jwt);
            if (jwt == null || String.IsNullOrWhiteSpace(jwt))
            {
                await HttpContext.SignOutAsync("Cookies");
                return LocalRedirectPermanent("~/Home");
            }

            await ExcelService.AddStudents(ExcelFiles, _rootPath, _uniAPI);
            return RedirectToAction("Index");
        }
    }
}