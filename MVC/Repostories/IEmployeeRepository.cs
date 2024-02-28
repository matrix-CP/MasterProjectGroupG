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
        //  List<tblEmployee> FetchEmployeeDetails();
        void AddEmployee(tblEmployee employee);

        List<tblEmployee> GetAllEmployee();

        List<tblEmployee> GetAllEmployeeDetails();

        tblEmployee GetEmployee(int id);
        void UpdateEmployee(tblEmployee updatedEmployee);
        void DeleteEmployee(int id);
        List<SelectListItem> GetDepartments();
        }
    }