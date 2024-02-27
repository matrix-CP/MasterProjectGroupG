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
         public bool Register(tblEmployee emp);
          public void Deleteemp(int id);
          public tblEmployee GetempById(int id);
           public void Updateemp(tblEmployee emp);
           public List<tblEmployee> Viewemp();

            public List<tblEmployee> Viewdept();

        
    }
}