using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UniversityRegister.API.Models;
using UniversityRegister.Models;

namespace UniversityRegister.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UniAPI _uniAPI;
        private readonly JwtService _jwtService;

        public LoginController(IConfiguration config, UniAPI uniAPI, JwtService jwtService)
        {
            _config = config;
            _uniAPI = uniAPI;
            _jwtService = jwtService;
        }

        // GET: Login
        public async Task<ActionResult> Index()
        {
            var token = new Jwt()
            {
                Token = HttpContext.Request.Cookies["Jwt"]
            };
            _uniAPI.SetJWT(token);
            await HttpContext.SignOutAsync("Cookies");

            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirectPermanent("~/Register");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind] Models.Credentials creds)
        {
            var apiResponse = await _uniAPI.Post<Jwt, Models.Credentials>("Login", creds);
            if (!(apiResponse.Result is EmptyResult))
            {
                var jwt = apiResponse.Value;
                if (!String.IsNullOrWhiteSpace(jwt.Token))
                {
                    _uniAPI.SetJWT(jwt);
                    
                    var cookieOptions = new CookieOptions()
                    {
                        Expires = DateTime.Now.AddHours(24)
                    };
                    Response.Cookies.Append("Jwt", jwt.Token, cookieOptions);
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        _jwtService.ValidateToken(jwt),
                        new AuthenticationProperties() { ExpiresUtc = DateTime.UtcNow.AddHours(24)});

                    return RedirectToRoutePermanent("Default", new { controller = "Register", action = "Index" });
                }
                else
                {
                    return LocalRedirectPermanent("~/Login");
                }
            }

            return LocalRedirectPermanent("~/Login");
        }
    }
}