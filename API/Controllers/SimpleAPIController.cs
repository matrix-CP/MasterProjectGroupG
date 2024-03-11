using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleAPIController : ControllerBase
    {
        private readonly ILogger<SimpleAPIController> _logger;
        private readonly IEmployeeRepository _emprepo;
        private readonly IUserRepository _userrepo;
        public SimpleAPIController(ILogger<SimpleAPIController> logger, IEmployeeRepository emprepo, IUserRepository userrepo)
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
        public IActionResult Login([FromBody]tblUser user)
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
        public IActionResult Register([FromBody]tblUser user)
        {
            _userrepo.Register(user);
            return Ok(true);
        }

        [HttpGet("GetAllEmployee")]
        public List<tblEmployee> GetAllEmp()
        {
            return _emprepo.GetAllEmployee();
        }

        [HttpGet("GetEmployee/{id}")]
        public tblEmployee GetOneEmp(int id)
        {
            return _emprepo.GetEmployee(id);
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
           _emprepo.AddEmployee(emp);
           return Ok(true);
        }

        [HttpPut]
        [Route("UpdateEmployee/")]
        public void Edit([FromForm]tblEmployee emp)
        {

            _emprepo.UpdateEmployee(emp);
        }

        [HttpDelete("delete/{id}")]
        public void Delete(int id)
        {
            _emprepo.DeleteEmployee(id);
            
        }
        
    }
}