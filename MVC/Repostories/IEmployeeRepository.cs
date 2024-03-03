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

        void AddEmployeeDetails(tblEmployee employee);

        List<tblEmployee> GetAllEmployeeUser();

        List<tblEmployee> GetAllEmployeeDetailsAdmin();

        tblEmployee GetEmployeeAdmin(int id);
        void UpdateEmployeeAdmin(tblEmployee updatedEmployee);

        bool AddEmployeeGrid(tblEmployee emp);
        void Deleteemp(int id);
        tblEmployee GetempById(int id);
        void Updateemp(tblEmployee emp);
        List<tblEmployee> viewemp();
        List<tblDepartment> Viewdept();

    }
}