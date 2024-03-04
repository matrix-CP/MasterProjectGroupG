using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repostories;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class SimpleMVCController : Controller
    {
        private readonly ILogger<SimpleMVCController> _logger;
        private readonly IEmployeeRepository _empRepo;
        private readonly IUserRepository _userRepo;

        public SimpleMVCController(ILogger<SimpleMVCController> logger, IEmployeeRepository empRepo, IUserRepository userRepo)
        {
            _logger = logger;
            _empRepo = empRepo;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Admin()
        {
            var employee =_empRepo.GetAllEmployee();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Employee()
        {
            var employee =_empRepo.GetAllEmployee();
            return View(employee);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            var employee = new tblEmployee();
            var dep = _empRepo.GetDepartments();
            employee.depList = dep;
            return View(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(tblEmployee employee)
        {
            _empRepo.AddEmployee(employee);
            return RedirectToAction("Employee");
        }

        [HttpPost]
        public IActionResult UpdateEmployee(tblEmployee employee)
        {
            _empRepo.UpdateEmployee(employee);
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult UserUpdateEmployee(tblEmployee employee)
        {
            _empRepo.UpdateEmployee(employee);
            return RedirectToAction("Employee");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _empRepo.DeleteEmployee(id);
            return RedirectToAction("Admin");
        }

         [HttpPost]
        public IActionResult UserDelete(int id)
        {
            _empRepo.DeleteEmployee(id);
            return RedirectToAction("Employee");
        }

        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            var employee = _empRepo.GetEmployee(id);
            var dep = _empRepo.GetDepartments();
            employee.depList = dep;
            return View(employee);
        }

        [HttpGet]
        public IActionResult UserUpdateEmployee(int id)
        {
            var employee = _empRepo.GetEmployee(id);
            var dep = _empRepo.GetDepartments();
            employee.depList = dep;
            return View(employee);
        }

        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _empRepo.GetEmployee(id);
            return View(employee);
        }

        [HttpGet]
        public IActionResult UserDeleteEmployee(int id)
        {
            var employee = _empRepo.GetEmployee(id);
            return View(employee);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _empRepo.GetEmployee(id);
            return View(employee);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(tblUser user)
        {
            tblUser users = _userRepo.Login(user);

            if(users.c_uid != 0)
            {
                if(users.c_role.Equals("admin"))
                {
                    return RedirectToAction("Admin");
                }else{
                    return RedirectToAction("Employee");
                }
                
            }

            return RedirectToAction("Login");
            
        }
        [HttpPost]
        public IActionResult Register(tblUser user)
        {
            _userRepo.Register(user);
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}