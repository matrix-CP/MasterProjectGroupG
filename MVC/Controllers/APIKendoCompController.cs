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
    public class APIKendoCompController : Controller
    {
        private readonly ILogger<APIKendoCompController> _logger;

        public APIKendoCompController(ILogger<APIKendoCompController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // [HttpPost("Login")]
        public IActionResult Login()
        {
           return View();
        }
           
        public IActionResult Register()
        {
            return View();
        }

        
        public IActionResult Admin()
        {
            return View();
        }

         public IActionResult Employee()
        {
            return View();
        }
      
        public IActionResult GetOneEmp( )
        {
           return View();
        }

        public IActionResult Details(int id){
             ViewBag.Id=id;
           return View();
        }
         
        public IActionResult GetPath()
        {
            return View();
        }

        
        public IActionResult GetDept()
        {
            return View();
        }

      
        public IActionResult AddEmployee()
        {
           return View();
        }

        
        public IActionResult UpdateEmployee(int id)
        {
            ViewBag.Id=id;
           return View();
        }

        public IActionResult UserUpdateEmployee(int id)
        {
            ViewBag.Id=id;
            
           return View();
        }
       
        public IActionResult Delete(int id)
        {
           ViewBag.Id=id;
           return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}