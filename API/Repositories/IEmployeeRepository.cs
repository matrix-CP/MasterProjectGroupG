using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Repositories
{
    public interface IEmployeeRepository
    {
        public void AddEmployee(tblEmployee employee);

        public void DeleteEmployee(int id);

        public List<tblEmployee> GetAllEmployee();

        public List<SelectListItem> GetDepartments();

        public tblEmployee GetEmployee(int id);

        public string GetExistingPath(int id);

        public void UpdateEmployee(tblEmployee employee);
    }
}