using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Repostories
{
    public interface IUserRepository
    {
        public void Register(tblUser user);
        public tblUser Login(tblUser user);
        void Registermvc(tblUser user);

        tblUser Loginmvc(tblUser user);
    }
}