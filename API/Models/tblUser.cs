using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class tblUser
    {
        public int c_uid{get; set;}
        public string? c_uname{get; set;}
        public string c_uemail{get; set;}
        public string c_password{get; set;}
        public string? c_chkpassword{get; set;}
        public string? c_role{get; set;}
    }
}