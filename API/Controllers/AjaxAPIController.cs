using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using API.Repositories;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AjaxAPIController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserRepository _userRepository;

        private readonly IEmployeeRepository _employeeRepository;

        public AjaxAPIController(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("Registermvc")]
        public IActionResult Registermvc(tblUser user)
        {
            try
            {
                _userRepository.Registermvc(user);
                return Ok("User Registered Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Loginmvc")]
        public IActionResult Loginmvc(tblUser user)
        {
            tblUser user1 = _userRepository.Loginmvc(user);
            if (user1.c_uid != 0 && !string.IsNullOrEmpty(user1.c_uname) && !string.IsNullOrEmpty(user1.c_uemail))
            {
                if (user1.c_role == "admin")
                {
                    HttpContext.Session.SetString("username", user1.c_uname);
                    HttpContext.Session.SetString("email", user1.c_uemail);
                    return Ok(new { status = "Admin", id = user1.c_uid, username = user1.c_uname, email = user1.c_uemail });
                }
                else
                {
                    HttpContext.Session.SetString("username", user1.c_uname);
                    HttpContext.Session.SetString("email", user1.c_uemail);
                    return Ok(new { status = "Success", id = user1.c_uid, username = user1.c_uname, email = user1.c_uemail });
                }
            }
            else
            {
                return BadRequest("Invalid Username or Password");
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            var session = HttpContext.Session;
            session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromForm] tblEmployee employee)
        {
            try
            {
                _employeeRepository.AddEmployeeDetails(employee);
                return Ok("Employee Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee(int user_id)
        {
            try
            {
                List<tblEmployee> employees = _employeeRepository.GetAllEmployeeUser(user_id);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeById")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                tblEmployee employee = _employeeRepository.GetEmployeeAdmin(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateEmployee")]
        public IActionResult UpdateEmployee([FromForm] tblEmployee employee)
        {
            try
            {
                _employeeRepository.UpdateEmployeeAdmin(employee);
                return Ok("Employee Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteEmployee")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                _employeeRepository.DeleteEmployee(id);
                return Ok("Employee Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDepartments")]
        public IActionResult GetDepartments()
        {
            try
            {
                List<SelectListItem> departments = _employeeRepository.GetDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllEmployeeDetails")]
        public IActionResult GetAllEmployeeDetails()
        {
            try
            {
                List<tblEmployee> employees = _employeeRepository.GetAllEmployeeDetailsAdmin();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")))
            {
                return RedirectToAction("Login");
            }

            return Ok();
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
              var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")))
            {
                return RedirectToAction("Login");
            }

            return Ok();
        }



    }
}