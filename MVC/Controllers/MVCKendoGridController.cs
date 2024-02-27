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
      private readonly IEmployeeRepository _cityRepositories;
       private readonly ILogger<MVCKendoGridController> _logger;
       public MVCKendoGridController(ILogger<MVCKendoGridController> logger,IEmployeeRepository cityRepositories)
        {
            _logger = logger;
            _cityRepositories = cityRepositories;
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
           public IActionResult ViewCity()
            {
                var citys = _cityRepositories.ViewCity();
                return Json(citys);
            }
            

             public IActionResult Viewdept()
            {
                var citys = _cityRepositories.Viewdept();
                return Json(citys);
            }

            
             public IActionResult Register()
                {
                    return View();
                }

        [HttpPost]
        public IActionResult Register(tblEmployee ct)
        {
            _cityRepositories.Register(ct); 
            return Json(new {success=true, message="Inserted", newCityId=ct.c_empid});
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var city = _cityRepositories.GetCityById(id);
            return View(city);
        }

        [HttpPost]
        public IActionResult Edit(tblEmployee city)
        {
             _cityRepositories.UpdateCity(city);
            return Json(new {success=true, Message="Updated Successfully", newCityId=city.c_empid});
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
    
}