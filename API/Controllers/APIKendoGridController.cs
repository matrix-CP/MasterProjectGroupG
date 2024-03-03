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
       private readonly IWebHostEnvironment _hostingEnvironment;



        public APIKendoGridController(IEmployeeRepository empRepo,IWebHostEnvironment hostingEnvironment )
        {
            _empRepo = empRepo;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                

                string uniqueFileName = Guid.NewGuid().ToString() + file.FileName;
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", uniqueFileName);
                


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string imageUrl = $"/Images/{uniqueFileName}";
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpGet]
        [Route("FetchAllItem")]
        public IActionResult GetAllItem()
        {
            
            return Ok( _empRepo.viewemp());
        }

        [HttpGet]
        [Route("FetchAlldept")]
        public IActionResult Viewdept()
        {
            
            return Ok( _empRepo.Viewdept());
        }

        [HttpPut]
        
        [Route("AddEmployeeGrid")]
        public IActionResult AddEmployeeGrid([FromBody]tblEmployee emp)
        {

            _empRepo.AddEmployeeGrid(emp);            
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