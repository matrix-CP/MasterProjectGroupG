using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repostories;
using Npgsql; 

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class MVCKendoGridController : Controller
    {
     private readonly NpgsqlConnection conn;
      private readonly IEmployeeRepository _empRepositories;
       private readonly ILogger<MVCKendoGridController> _logger;
       public MVCKendoGridController(ILogger<MVCKendoGridController> logger,IEmployeeRepository empRepositories)
        {
            _logger = logger;
            _empRepositories = empRepositories;
        }

         public IActionResult Index()
            {
                return View();
            }
             public IActionResult Apiindex()
            {
                return View();
            }


            [Produces("application/json")]
           public IActionResult Viewemp()
            {
                var emps = _empRepositories.Viewemp();
                return Json(emps);
            }
            

             public IActionResult Viewdept()
            {
                var emps = _empRepositories.Viewdept();
                return Json(emps);
            }

            
             public IActionResult Register()
                {
                    return View();
                }

        [HttpPost]
        public IActionResult Register(tblEmployee emp)
        {
            _empRepositories.Register(emp); 
            return Json(new {success=true, message="Inserted", newEMPId=emp.c_empid});
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var emp = _empRepositories.GetempById(id);
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(tblEmployee emp)
        {
             _empRepositories.Updateemp(emp);
            return Json(new {success=true, Message="Updated Successfully", newEMPId=emp.c_empid});
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
    
}