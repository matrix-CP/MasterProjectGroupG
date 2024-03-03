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
    public class MVCKendoCompController : Controller
    {
        private readonly ILogger<MVCKendoCompController> _logger;
         private readonly IUserRepository _userrepo;
         private readonly IEmployeeRepository _empRepo;

        public MVCKendoCompController(ILogger<MVCKendoCompController> logger, IUserRepository userrepo, IEmployeeRepository empRepo)
        {
            _logger = logger;
             _userrepo = userrepo;
             _empRepo = empRepo;
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
            tblUser users = _userrepo.Login(user);
            Console.WriteLine(users != null);
            if(users != null)
            {
                if(users.c_role.Equals("user"))
                {
                    return Json("Employee");
                }else{
                    return Json("Admin");
                }
                
            }

            return Json("Invalid Credentials");
            
        }
        [HttpPost]
        public IActionResult Register(tblUser user)
        {
            _userrepo.Register(user);
            return Json("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}