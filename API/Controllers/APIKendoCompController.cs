using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIKendoCompController : ControllerBase
    {
        private readonly ILogger<APIKendoCompController> _logger;

        private readonly IEmployeeRepository _emprepo;
        private readonly IUserRepository _userrepo;
         public APIKendoCompController(ILogger<APIKendoCompController> logger, IEmployeeRepository emprepo, IUserRepository userrepo)
        {
            _logger = logger;
            _emprepo = emprepo; 
            _userrepo = userrepo; 
        }

          // [HttpGet]
        // public IActionResult Login()
        // {
        //     return View();
        // }

        [HttpPost("Login")]
        public IActionResult Login([FromForm]tblUser user)
        {
            tblUser users = _userrepo.Login(user);
            Console.WriteLine(users != null);
            if(users != null)
            {
                if(users.c_role.Equals("admin"))
                {
                    return Ok("Admin");
                } else{
                    return Ok("Employee");
                } 
            }
            return Ok("Invalid Credentials!"); 
        }


        // [HttpGet]
        // public IActionResult Register()
        // {
        //     return View();
        // }

        [HttpPost("Register")]
        public IActionResult Register([FromForm]tblUser user)
        {
            _userrepo.Register(user);
            return Ok(true);
        }

         [HttpGet("Admin")]
        public IActionResult Admin()
        {
            try
            {
                var employees = _emprepo.GetAllEmployee();
                if (employees != null && employees.Any())
                    return Ok(employees);

                return NotFound("No employees found");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting employees: {ex.Message}");
            }
        }

        [HttpGet("Employee")]
        public IActionResult Employee()
        {
            try
            {
                var employees = _emprepo.GetAllEmployee();
                if (employees != null && employees.Any())
                    return Ok(employees);

                return NotFound("No employees found");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting employees: {ex.Message}");
            }
        }


      [HttpGet("GetEmployee/{id}")]
        public IActionResult GetOneEmp(int id)
        {
            try
            {
                var employee = _emprepo.GetEmployee(id);

                if (employee != null)
                    return Ok(employee);

                return NotFound($"Employee with ID {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


         [HttpGet("GetPath/{id}")]
        public string GetPath(int id)
        {
            return _emprepo.GetExistingPath(id);
        }

        [HttpGet("GetDept")]
        public List<SelectListItem> GetDept()
        {
            return _emprepo.GetDepartments();
        }

        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult Add([FromForm]tblEmployee emp)
        {
            try
            {
                _emprepo.AddEmployee(emp);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateEmployee/")]
        public IActionResult UpdateEmployee([FromForm]tblEmployee emp)
        {
            try
            {
                _emprepo.UpdateEmployee(emp);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UserUpdateEmployee")]
        public IActionResult UserUpdateEmployee([FromForm] tblEmployee employee)
        {
            try
            {
                _emprepo.UpdateEmployee(employee);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _emprepo.DeleteEmployee(id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}