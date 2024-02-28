using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Models
{
    public class tblEmployee
    {
        public int c_empid{get; set;}
        public string c_empname{get; set;}
        public string c_empgender{get; set;}
        public DateTime c_dob{get; set;}
        public List<string> c_shift{get; set;}
        public int c_depart{get; set;}
        public string depname{get; set;}
        public List<SelectListItem>? depList{get; set;}
        public string c_img{get; set;}
        public IFormFile imgFile{get; set;}
        public int c_uid{get; set;}
        public string c_username{get; set;}

    }
}