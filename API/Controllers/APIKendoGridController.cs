using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Repositories; 

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIKendoGridController : ControllerBase
    {
         private readonly IEmployeeRepository _empRepo;
       // private static string _uploadedFileName;

        public APIKendoGridController(IEmployeeRepository empRepo)
        {
            _empRepo = empRepo;
        }

         [HttpGet]
        [Route("FetchAllItem")]
        public IActionResult GetAllItem()
        {
            
            return Ok( _empRepo.Viewemp());
        }

          [HttpGet]
        [Route("FetchAlldept")]
        public IActionResult Viewdept()
        {
            
            return Ok( _empRepo.Viewdept());
        }

        [HttpPut]
        
        [Route("Register")]
        public IActionResult Register([FromBody]tblEmployee emp)
        {

            _empRepo.Register(emp);            
            return CreatedAtAction(nameof(GetempById), new { id = emp.c_empid }, emp );
        }

         [HttpDelete]
        [Route("delete/{id}")]
         public IActionResult Deleteemp(int id)
        {
            _empRepo.Deleteemp(id);
            return Ok("Delete Successfully");
        }

         [HttpGet]
        [Route("GetempById/{id}")]
         public IActionResult GetempById(int id)
        {
            var emp = _empRepo.GetempById(id);
            if(emp == null)
            {
                return NotFound("emp not found");
            }
            return Ok(emp);
        }

         [HttpPut]
        [Route("edit/{id}")]
         public IActionResult Updateemp(int id,[FromBody] tblEmployee emp)
        {
            _empRepo.Updateemp(emp);
            return Ok("Update Successfully");
        }


        
    }
}