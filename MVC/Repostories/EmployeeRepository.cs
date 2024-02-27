using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using Npgsql;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MVC.Repostories
{
    public class EmployeeRepository : CommonRepository , IEmployeeRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public bool IsAuthenticated
        {
            get
            {
                var session = _httpContextAccessor.HttpContext.Session;
                return session.GetInt32("IsAuthenticated") == 1;
            }
        }

        public EmployeeRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddEmployee(tblEmployee employee)
        {
            if(employee.imgFile != null && employee.imgFile.Length > 0)
            {
                var folderPath = "D://casepoint//master//MasterProjectGroupG//MVC//wwwroot//Images";
                // var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Images");
                var filePath = Guid.NewGuid().ToString()+employee.imgFile.FileName;
                var fullPath = Path.Combine(folderPath, filePath);
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    employee.imgFile.CopyTo(stream);
                }

                employee.c_img = "/Images/"+filePath;

            }
            else
            {
                employee.c_img = "/Images/default.png";
            }

            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO t_empmaster (c_empname, c_empgender, c_dob, c_shift, c_depart, c_img, c_uid) VALUES(@name, @gender, @dob, @shift, @dept, @img, @uid)";
            cmd.Parameters.AddWithValue("@name", employee.c_empname);
            cmd.Parameters.AddWithValue("@gender", employee.c_empgender);
            cmd.Parameters.AddWithValue("@dob", employee.c_dob);
            string shifts = string.Join(',', employee.c_shift);
            cmd.Parameters.AddWithValue("@shift", shifts);
            cmd.Parameters.AddWithValue("@dept", employee.c_depart);
            cmd.Parameters.AddWithValue("@img", employee.c_img);
            // cmd.Parameters.AddWithValue("@uid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
           cmd.Parameters.AddWithValue("@uid", 1);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        

        
    }
}