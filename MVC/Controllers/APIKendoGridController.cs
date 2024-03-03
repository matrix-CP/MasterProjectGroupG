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
    public class APIKendoGridController : Controller
    {
        private readonly ILogger<APIKendoGridController> _logger;

        public APIKendoGridController(ILogger<APIKendoGridController> logger)
        {
            _logger = logger;
        }

        public IActionResult APIIndex()
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