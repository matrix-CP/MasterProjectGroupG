using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repostories;

namespace MVC.Controllers
{
    public class MVCAjaxController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public MVCAjaxController(IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")))
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
                return Json("Success");
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

        [HttpPost]
        public JsonResult DeleteEmployee(int id)
        {
            _employeeRepository.DeleteEmployee(id);
            return Json("Employee Deleted Successfully");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }



        [HttpGet]
        public JsonResult GetAllEmployeeDetails()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployeeDetails();
            return Json(employees);
        }

        [HttpGet]
        public JsonResult GetEmployee(int id)
        {
            tblEmployee employee = _employeeRepository.GetEmployee(id);
            return Json(employee);
        }

        
        [HttpPost]
        public JsonResult UpdateEmployee([FromBody] tblEmployee employee)
        {
            _employeeRepository.UpdateEmployee(employee);
            return Json("Employee Updated Successfully");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
