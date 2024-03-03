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
        public void AddEmployeeDetails(tblEmployee employee);
        public List<tblEmployee> GetAllEmployeeUser(int user_id);
        public List<tblEmployee> GetAllEmployeeDetailsAdmin();
        public tblEmployee GetEmployeeAdmin(int id);
        public void UpdateEmployeeAdmin(tblEmployee updatedEmployee);

        bool AddEmployeeGrid(tblEmployee emp);
        void Deleteemp(int id);
        tblEmployee GetempById(int id);
        void Updateemp(tblEmployee emp);
        List<tblEmployee> viewemp();
        List<tblDepartment> Viewdept();
    }
}