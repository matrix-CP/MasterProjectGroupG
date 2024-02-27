using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repostories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;


namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class MVCAjaxController : Controller
    {
        private readonly ILogger<MVCAjaxController> _logger;

        private readonly IUserRepository _userRepository;

        private readonly IEmployeeRepository _employeeRepository;

        public MVCAjaxController(ILogger<MVCAjaxController> logger, IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")) )
            {
                return RedirectToAction("Login");
            }

         
            return View();

        }
        public JsonResult GetAllEmployee()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployee();
            return Json(employees);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register([FromBody] tblUser user)
        {
            _userRepository.Register(user);
            return Json("Registered Successfully");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login([FromBody] tblUser user)
        {
            tblUser user1 = _userRepository.Login(user);
            if (user1.c_uid != 0)
            {
                // HttpContext.Session.SetInt32("userid", user1.c_uid);
                // HttpContext.Session.SetInt32("IsAuthenticated", 1);
                return Json("Success"); // Change this line to return "Success"
            }
            else
            {
                return Json("Invalid Username or Password");
            }
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddEmployee([FromForm] tblEmployee employee)
        {
            _employeeRepository.AddEmployee(employee);
            return Json("Employee Added Successfully");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}