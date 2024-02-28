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
        void AddEmployeeDetails(tblEmployee employee);

        List<tblEmployee> GetAllEmployeeUser();

        List<tblEmployee> GetAllEmployeeDetailsAdmin();

        tblEmployee GetEmployeeAdmin(int id);
        void UpdateEmployeeAdmin(tblEmployee updatedEmployee);
        void DeleteEmployee(int id);
        List<SelectListItem> GetDepartments();
        }
    }