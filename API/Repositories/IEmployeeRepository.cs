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
        void AddEmployeeDetails(tblEmployee employee);

        List<tblEmployee> GetAllEmployeeUser();

        List<tblEmployee> GetAllEmployeeDetailsAdmin();

        tblEmployee GetEmployeeAdmin(int id);
        void UpdateEmployeeAdmin(tblEmployee updatedEmployee);
        void DeleteEmployee(int id);
        List<SelectListItem> GetDepartments();
    }
}