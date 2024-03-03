using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class APIAjaxController : Controller
    {
        private readonly ILogger<APIAjaxController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public APIAjaxController(ILogger<APIAjaxController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // [Route("Index")]
        public IActionResult Index(int user_id, string username)
        {
            //var username = HttpContext.Session.GetString("username");
            // var email = HttpContext.Session.GetString("email");

            ViewData["Username"] = username;
            // ViewData["Email"] = email;
            ViewData["user_id"] = user_id;
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Dashboard(int user_id, string username)
        {
            ViewData["Username"] = username;

            ViewData["user_id"] = user_id;
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var session = HttpContext.Session;
            session.Clear();
            return RedirectToAction("Login");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}