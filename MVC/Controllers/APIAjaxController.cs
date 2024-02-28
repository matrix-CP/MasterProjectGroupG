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
        public IActionResult Index()
        {
            // var username = HttpContext.Session.GetString("username");
            // var email = HttpContext.Session.GetString("email");

            // ViewData["Username"] = username;
            // ViewData["Email"] = email;

            return View();
        }


        // [Route("Login")]
        public IActionResult Login()
        {

            return View();
        }

        // [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}