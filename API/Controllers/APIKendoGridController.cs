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
         private readonly IEmployeeRepository _cityRepo;
       // private static string _uploadedFileName;

        public APIKendoGridController(IEmployeeRepository cityRepo)
        {
            _cityRepo = cityRepo;
        }

         [HttpGet]
        [Route("FetchAllItem")]
        public IActionResult GetAllItem()
        {
            
            return Ok( _cityRepo.ViewCity());
        }

          [HttpGet]
        [Route("FetchAlldept")]
        public IActionResult Viewdept()
        {
            
            return Ok( _cityRepo.Viewdept());
        }

        [HttpPut]
        
        [Route("Register")]
        public IActionResult Register([FromBody]tblEmployee ct)
        {

            _cityRepo.Register(ct);            
            return CreatedAtAction(nameof(GetCityById), new { id = ct.c_empid }, ct );
        }

         [HttpDelete]
        [Route("delete/{id}")]
         public IActionResult DeleteCity(int id)
        {
            _cityRepo.DeleteCity(id);
            return Ok("Delete Successfully");
        }

         [HttpGet]
        [Route("GetCityById/{id}")]
         public IActionResult GetCityById(int id)
        {
            var cities = _cityRepo.GetCityById(id);
            if(cities == null)
            {
                return NotFound("City not found");
            }
            return Ok(cities);
        }

         [HttpPut]
        [Route("edit/{id}")]
         public IActionResult UpdateCity(int id,[FromBody] tblEmployee city)
        {
            _cityRepo.UpdateCity(city);
            return Ok("Update Successfully");
        }


        
    }
}