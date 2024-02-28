using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Npgsql;
using API.Repositories;

namespace API.Repositories
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

        public void AddEmployeeDetails(tblEmployee employee)
        {
            if (employee.imgFile != null && employee.imgFile.Length > 0)
            {
                var folderPath = "D://casepoint//master//MasterProjectGroupG//MVC//wwwroot//Images";
                // var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Images");
                var filePath = Guid.NewGuid().ToString() + employee.imgFile.FileName;
                var fullPath = Path.Combine(folderPath, filePath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    employee.imgFile.CopyTo(stream);
                }

                employee.c_img = "/Images/" + filePath;

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


        public List<tblEmployee> GetAllEmployeeUser()
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

        public List<tblEmployee> GetAllEmployeeDetailsAdmin()
        {
            var employees = new List<tblEmployee>();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.CommandText = "SELECT e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname, e.c_uid, u.c_uname FROM t_empmaster e JOIN t_departmaster d ON e.c_depart = d.c_depid JOIN t_usermaster u ON e.c_uid = u.c_uid";
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
                    // c_uid = dr["c_uid"] != DBNull.Value ? Convert.ToInt32(dr["c_uid"]) : 0,
                    c_username = dr["c_uname"].ToString()
                };
                employees.Add(emp);
            }
            conn.Close();
            return employees;

        }

        public tblEmployee GetEmployeeAdmin(int id)
        {
            var employee = new tblEmployee();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname, e.c_uid, u.c_uname FROM t_empmaster e JOIN t_departmaster d ON e.c_depart = d.c_depid JOIN t_usermaster u ON e.c_uid = u.c_uid WHERE c_empid = @id";
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
                // employee.c_uid = dr["c_uid"] != DBNull.Value ? Convert.ToInt32(dr["c_uid"]) : 0;
                employee.c_username = dr["c_uname"].ToString(); 

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


        public void UpdateEmployeeAdmin(tblEmployee updatedEmployee)
        {
            try
            {

                if (updatedEmployee.imgFile != null && updatedEmployee.imgFile.Length > 0)
                {
                    var folderPath = "D://casepoint//master//MasterProjectGroupG//MVC//wwwroot//Images";
                    // var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Images");
                    var filePath = Guid.NewGuid().ToString() + updatedEmployee.imgFile.FileName;
                    var fullPath = Path.Combine(folderPath, filePath);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        updatedEmployee.imgFile.CopyTo(stream);
                    }

                    updatedEmployee.c_img = "/Images/" + filePath;

                }
                else
                {
                    updatedEmployee.c_img = GetExistingPath(updatedEmployee.c_empid);
                }
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "UPDATE t_empmaster SET c_empname = @name, c_empgender = @gender, c_dob = @dob, c_img = @img, c_depart = @depart, c_shift = @shift WHERE c_empid = @empid";

                    cmd.Parameters.AddWithValue("@name", updatedEmployee.c_empname);
                    cmd.Parameters.AddWithValue("@gender", updatedEmployee.c_empgender);
                    cmd.Parameters.AddWithValue("@dob", updatedEmployee.c_dob);
                    cmd.Parameters.AddWithValue("@img", updatedEmployee.c_img);
                    cmd.Parameters.AddWithValue("@depart", updatedEmployee.c_depart);
                    cmd.Parameters.AddWithValue("@shift", string.Join(",", updatedEmployee.c_shift));
                    cmd.Parameters.AddWithValue("@empid", updatedEmployee.c_empid);
                    cmd.Parameters.AddWithValue("@uid", updatedEmployee.c_uid);

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetExistingPath(int id)
        {
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select c_img from t_empmaster WHERE c_empid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            var imgPath = cmd.ExecuteScalar().ToString();
            conn.Close();
            return imgPath;
        }

        public List<SelectListItem> GetDepartments()
        {
            var departments = new List<SelectListItem>();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from t_departmaster";
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var dep = new SelectListItem
                {
                    Text = dr["c_depname"].ToString(),
                    Value = dr["c_depid"].ToString()
                };
                departments.Add(dep);
            }
            conn.Close();
            return departments;
        }


    }
}