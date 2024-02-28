using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            List<tblEmployee> employees = _employeeRepository.GetAllEmployeeUser();
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
            _userRepository.Registermvc(user);
            return Json("Registered Successfully");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var session = HttpContext.Session;
            session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public JsonResult Login([FromBody] tblUser user)
        {
            tblUser user1 = _userRepository.Loginmvc(user);

            if (user1.c_uid != 0)
            {
                if (user1.c_role == "admin")
                {
                    // For admin user
                    return Json("Admin");
                }
                else
                {
                    // For other users
                    return Json("Success");
                }
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
            _employeeRepository.AddEmployeeDetails(employee);
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
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")))
            {
                return RedirectToAction("Login");
            }

            return View();
        }



        [HttpGet]
        public JsonResult GetAllEmployeeDetails()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployeeDetailsAdmin();
            return Json(employees);
        }

        [HttpGet]
        public JsonResult GetEmployee(int id)
        {
            tblEmployee employee = _employeeRepository.GetEmployeeAdmin(id);
            return Json(employee);
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            List<SelectListItem> departments = _employeeRepository.GetDepartments();
            return Json(departments);
        }


        [HttpPost]
        public JsonResult UpdateEmployee([FromForm] tblEmployee employee)
        {
            _employeeRepository.UpdateEmployeeAdmin(employee);
            return Json("Employee Updated Successfully");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");

        }


    }
}
