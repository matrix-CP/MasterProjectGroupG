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
        private readonly IEmployeeRepository _emprepo;
        private readonly IUserRepository _userrepo;
        public SimpleAPIController(IEmployeeRepository emprepo, IUserRepository userrepo)
        {
            _emprepo = emprepo; 
            _userrepo = userrepo; 
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