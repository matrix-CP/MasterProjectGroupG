using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;

namespace MVC.Repostories
{
    public interface IEmployeeRepository
    {

        public List<tblEmployee> GetAllEmployee();
        public List<SelectListItem> GetDepartments();
        public tblEmployee GetEmployee(int id);
        public void UpdateEmployee(tblEmployee employee);
        public void AddEmployee(tblEmployee employee);
        public void DeleteEmployee(int id);

    }
}