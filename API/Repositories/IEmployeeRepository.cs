using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; 
using API.Repositories;

namespace API.Repositories
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