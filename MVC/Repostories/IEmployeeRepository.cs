using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models; 
using MVC.Repostories; 

namespace MVC.Repostories
{
    public interface IEmployeeRepository
    {
         public bool Register(tblEmployee ct);
          public void DeleteCity(int id);
          public tblEmployee GetCityById(int id);
           public void UpdateCity(tblEmployee city);
           public List<tblEmployee> ViewCity();
            public tblEmployee GetOneCity(int c_empid);
            public List<tblEmployee> Viewdept();

        
    }
}