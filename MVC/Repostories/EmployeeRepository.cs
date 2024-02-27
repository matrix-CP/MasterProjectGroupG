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
             cmd.Parameters.AddWithValue("@uid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
           //cmd.Parameters.AddWithValue("@uid", 1);
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public List<tblEmployee> GetAllEmployee()
        {
            var employees = new List<tblEmployee>();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.CommandText = "select e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname from t_empmaster e join t_departmaster d on e.c_depart = d.c_depid where e.c_uid = @uid";
            cmd.Parameters.AddWithValue("@uid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var emp = new tblEmployee
                {
                    c_empid = Convert.ToInt32(dr["c_empid"]),
                    c_empname = dr["c_empname"].ToString(),
                    c_empgender = dr["c_empgender"].ToString(),
                    c_dob = DateTime.Parse(dr["c_dob"].ToString()),
                    c_shift = dr["c_shift"].ToString().Split(",").ToList(),
                    // c_depart = Convert.ToInt32(dr["c_depart"]),
                    c_img = dr["c_img"].ToString(),
                    depname = dr["c_depname"].ToString()


                };
                employees.Add(emp);
            }
            conn.Close();
            return employees;

        }

        public List<tblEmployee> GetAllEmployeeDetails()
        {
            var employees = new List<tblEmployee>();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.CommandText = "select e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, e.uid, d.c_depname from t_empmaster e join t_departmaster d on e.c_depart = d.c_depid";
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var emp = new tblEmployee
                {
                    c_empid = Convert.ToInt32(dr["c_empid"]),
                    c_empname = dr["c_empname"].ToString(),
                    c_empgender = dr["c_empgender"].ToString(),
                    c_dob = DateTime.Parse(dr["c_dob"].ToString()),
                    c_shift = dr["c_shift"].ToString().Split(",").ToList(),
                    // c_depart = Convert.ToInt32(dr["c_depart"]),
                    c_img = dr["c_img"].ToString(),
                    depname = dr["c_depname"].ToString(),
                    c_uid = Convert.ToInt32(dr["uid"])
                };
                employees.Add(emp);
            }
            conn.Close();
            return employees;

        }

        public tblEmployee GetEmployee(int id)
        {
            var employee = new tblEmployee();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname from t_empmaster e join t_departmaster d on e.c_depart = d.c_depid WHERE c_empid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                employee.c_empid = Convert.ToInt32(dr["c_empid"]);
                employee.c_empname = dr["c_empname"].ToString();
                employee.c_empgender = dr["c_empgender"].ToString();
                employee.c_dob = DateTime.Parse(dr["c_dob"].ToString());
                employee.c_shift = dr["c_shift"].ToString().Split(",").ToList();
                employee.c_depart = Convert.ToInt32(dr["c_depart"]);
                employee.c_img = dr["c_img"].ToString();

            }
            conn.Close();
            return employee;
        }

        public void DeleteEmployee(int id)
        {
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "DELETE FROM t_empmaster WHERE c_empid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }





        

        
    }
}