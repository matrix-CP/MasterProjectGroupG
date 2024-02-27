using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using Npgsql;

namespace MVC.Repostories
{
    public class EmployeeRepository : CommonRepository, IEmployeeRepository
    {
        public void AddEmployee(tblEmployee employee)
        {
            if(employee.imgFile != null && employee.imgFile.Length > 0)
            {
                // var folderPath = "C:\Users\bhatt\OneDrive\Desktop\Casepoint Internship\GitDemo\MasterProject\MasterProjectGroupG\MVC\wwwroots";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Images");
                // var filePath = Guid.NewGuid().ToString()+employee.imgFile.FileName;
                var fullPath = Path.Combine(folderPath, employee.imgFile.FileName);
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    employee.imgFile.CopyTo(stream);
                }

                employee.c_img = "/Images/"+employee.imgFile.FileName;

            }
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO t_empmaster (c_empname, c_empgender, c_dob, c_shift, c_depart, c_img) VALUES(@name, @gender, @dob, @shift, @dept, @img)";
            cmd.Parameters.AddWithValue("@name", employee.c_empname);
            cmd.Parameters.AddWithValue("@gender", employee.c_empgender);
            cmd.Parameters.AddWithValue("@dob", employee.c_dob);
            string shifts = string.Join(',', employee.c_shift);
            cmd.Parameters.AddWithValue("@shift", shifts);
            cmd.Parameters.AddWithValue("@dept", employee.c_depart);
            cmd.Parameters.AddWithValue("@img", employee.c_img);
            cmd.ExecuteNonQuery();
            conn.Close();
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

        public List<tblEmployee> GetAllEmployee()
        {
            var employees = new List<tblEmployee>();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname * from t_empmaster";
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
                    c_depart = Convert.ToInt32(dr["c_depart"]),
                    c_img = dr["c_img"].ToString()
                };
                employees.Add(emp);
            }
            conn.Close();
            return employees;

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

        public tblEmployee GetEmployee(int id)
        {
            var employee = new tblEmployee();
            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from t_empmaster WHERE c_empid = @id";
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
        public void UpdateEmployee(tblEmployee employee)
        {
            if(employee.imgFile != null && employee.imgFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
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
            else{
                employee.c_img = GetExistingPath(employee.c_empid);
            }

            var cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "UPDATE t_empmaster SET c_empname=@name, c_empgender=@gender, c_dob=@dob, c_shift=@shift, c_depart=@dept, c_img=@img WHERE c_empid = @id";
            cmd.Parameters.AddWithValue("@id", employee.c_empid);
            cmd.Parameters.AddWithValue("@name", employee.c_empname);
            cmd.Parameters.AddWithValue("@gender", employee.c_empgender);
            cmd.Parameters.AddWithValue("@dob", employee.c_dob);
            string shifts = string.Join(',', employee.c_shift);
            cmd.Parameters.AddWithValue("@shift", shifts);
            cmd.Parameters.AddWithValue("@dept", employee.c_depart);
            cmd.Parameters.AddWithValue("@img", employee.c_img);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}