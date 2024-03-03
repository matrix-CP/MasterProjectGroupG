using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IUserRepository
    {
        public void Register(tblUser user);

        public tblUser Login(tblUser user);

        void Registermvc(tblUser user);

        tblUser Loginmvc(tblUser user);
    }
}