using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using Npgsql;

namespace MVC.Repostories
{
    public interface IUserRepository
    {
        void Register(tblUser user);

        tblUser Login(tblUser user);
    }
}