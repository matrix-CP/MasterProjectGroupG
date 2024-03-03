using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repostories;
using Npgsql;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class MVCKendoGridController : Controller
    {
        private readonly ILogger<MVCKendoGridController> _logger;
        private readonly IEmployeeRepository _empRepositories;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MVCKendoGridController(
            ILogger<MVCKendoGridController> logger,
            IEmployeeRepository empRepositories,
            IWebHostEnvironment hostingEnvironment
        )
        {
            _logger = logger;
            _empRepositories = empRepositories;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult UploadPhoto(tblEmployee emp)
        {
            if (emp.imgFile != null)
            {
                var filename = Guid.NewGuid().ToString() + emp.imgFile.FileName;
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", filename);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    emp.imgFile.CopyTo(stream);
                }
                file="/Images/"+filename;
            }
            return Json("Image Uploaded");
        }

        public IActionResult MVCIndex()
        {
            return View();
        }

        public IActionResult ApiIndex()
        {
            return View();
        }

        [Produces("application/json")]
        public IActionResult viewemp()
        {
            var emps = _empRepositories.viewemp();
            return Json(emps);
        }

        public IActionResult Viewdept()
        {
            var depts = _empRepositories.Viewdept();
            return Json(depts);
        }


        static string file = "";

        [HttpPost]
        public IActionResult AddEmployeeGrid(tblEmployee emp)
        {
            emp.c_img = file;

            _empRepositories.AddEmployeeGrid(emp);
            return Json(
                new
                {
                    success = true,
                    message = "Inserted",
                    newEMPId = emp.c_empid
                }
            );
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
            emp.c_img = file;

            _empRepositories.Updateemp(emp);
            return Json(
                new
                {
                    success = true,
                    Message = "Updated Successfully",
                    newEMPId = emp.c_empid
                }
            );
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
