 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UniversityRegister.Models;

namespace UniversityRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UniAPI _uniAPI;


        public HomeController(IConfiguration config, UniAPI uniAPI)
        {
            _config = config;
            _uniAPI = uniAPI;
        }

        public IActionResult Index()
        {
            var token = new Jwt()
            {
                Token = HttpContext.Request.Cookies["Jwt"]
            };
            _uniAPI.SetJWT(token);

            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirectPermanent("~/Register/");
            }
            else
            {
                return LocalRedirectPermanent("~/Login/");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
