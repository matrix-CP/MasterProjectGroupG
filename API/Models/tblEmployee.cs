using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class tblEmployee
    {
        public int c_empid{get; set;}
        public string c_empname{get; set;}
        public string c_empgender{get; set;}
        public DateTime c_dob{get; set;}
        public string c_shift{get; set;}
        public int c_depart{get; set;}
        public string c_depname{get; set;}
       // public List<SelectListItem> depList{get; set;}
        public string c_img{get; set;}
        public int c_depid{get; set;}
        public IFormFile imgFile{get; set;}

        
    }
}