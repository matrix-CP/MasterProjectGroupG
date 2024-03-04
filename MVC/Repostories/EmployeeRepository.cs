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

                // var folderPath = "D://My Learning//Core MVC//MasterProjectGroupG//MVC//wwwroot//Images";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Images");
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

            cmd.CommandText = "select e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname from t_empmaster e join t_departmaster d on e.c_depart = d.c_depid";

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

                    depname = dr["c_depname"].ToString(),

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

                employee.depname = dr["c_depname"].ToString();

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

        #region 2> Ajax 
        public void AddEmployeeDetails(tblEmployee employee)
        {
            if (employee.imgFile != null && employee.imgFile.Length > 0)
            {
               var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
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
                employee.c_uid = dr["c_uid"] != DBNull.Value ? Convert.ToInt32(dr["c_uid"]) : 0;
                employee.c_username = dr["c_uname"].ToString(); 

            }
            conn.Close();
            return employee;
        }
        
        public void UpdateEmployeeAdmin(tblEmployee updatedEmployee)
        {
            try
            {

                if (updatedEmployee.imgFile != null && updatedEmployee.imgFile.Length > 0)
                {
                     var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
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
                    // cmd.Parameters.AddWithValue("@uid", updatedEmployee.c_uid);

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                conn.Close();
            }
        }
    
        #endregion

        #region 3> KendoGrid
        public bool AddEmployeeGrid(tblEmployee emp)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO t_empmaster(c_empname, c_empgender, c_dob, c_shift, c_depart, c_img)VALUES (@c_empname, @c_empgender, @c_dob, @c_shift, @c_depart, @c_img);";
            cmd.Parameters.AddWithValue("@c_empname",emp.c_empname);
            cmd.Parameters.AddWithValue("@c_empgender",emp.c_empgender);
            cmd.Parameters.AddWithValue("@c_dob",emp.c_dob);
            string selectedShift=string.Join(',',emp.c_shift);
            cmd.Parameters.AddWithValue("@c_shift",selectedShift);
            cmd.Parameters.AddWithValue("@c_depart",emp.c_depart);
            cmd.Parameters.AddWithValue("@c_img",emp.c_img);
            int rowsAffected = cmd.ExecuteNonQuery();
            conn.Close();
            return rowsAffected > 0;
        }

        public void Deleteemp(int id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "DELETE FROM t_empmaster WHERE c_empid = @c_empid";
            cmd.Parameters.AddWithValue("@c_empid", id);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public tblEmployee GetempById(int id)
        {
            tblEmployee emp = new tblEmployee();
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_depart, c_img FROM t_empmaster WHERE c_empid = @c_empid";
            cmd.Parameters.AddWithValue("@c_empid", id);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                emp.c_empid = Convert.ToInt32(dr["c_empid"]);
                emp.c_empname = dr["c_empname"].ToString();
                emp.c_empgender = dr["c_empgender"].ToString();
                emp.c_dob = DateTime.Parse(dr["c_dob"].ToString());
                emp.c_shift = dr["c_shift"].ToString().Split(',').ToList();
                 emp.c_img = dr["c_img"].ToString();
                  emp.c_depart = Convert.ToInt32(dr["c_depart"]);
            }
            conn.Close();
            return emp;
        }

         public void Updateemp(tblEmployee emp)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "UPDATE t_empmaster SET c_empname = @c_empname ,c_empgender=@c_empgender ,c_dob=@c_dob,c_shift=@c_shift,c_img=@c_img,c_depart=@c_depart  WHERE c_empid = @c_empid";
            cmd.Parameters.AddWithValue("@c_empid", emp.c_empid);
            cmd.Parameters.AddWithValue("@c_empname", emp.c_empname);
             cmd.Parameters.AddWithValue("@c_empgender", emp.c_empgender);
            cmd.Parameters.AddWithValue("@c_dob", emp.c_dob);
            string selectedShift=string.Join(',',emp.c_shift);
             cmd.Parameters.AddWithValue("@c_shift", selectedShift);
            cmd.Parameters.AddWithValue("@c_img", emp.c_img);
            cmd.Parameters.AddWithValue("@c_depart", emp.c_depart);

            cmd.ExecuteNonQuery();
            conn.Close();
        }


         public List<tblEmployee> viewemp()
        {
            var emps = new List<tblEmployee>();
            conn.Open();

            using var command = new NpgsqlCommand("SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_depart, c_img FROM t_empmaster;", conn);

            command.CommandType = System.Data.CommandType.Text;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tblEmployee = new tblEmployee
                {
                c_empid = Convert.ToInt32(reader["c_empid"]),
                c_empname = reader["c_empname"].ToString(),
                c_empgender = reader["c_empgender"].ToString(),
                c_dob = DateTime.Parse(reader["c_dob"].ToString()),
                
                c_shift = reader["c_shift"].ToString().Split(',').ToList(),
                c_img = reader["c_img"].ToString(),
                c_depart = Convert.ToInt32(reader["c_depart"])
                };
                emps.Add(tblEmployee);
            }
            conn.Close();
            return emps;
        }

        public List<tblDepartment> Viewdept()
        {
            var depts = new List<tblDepartment>();
            conn.Open();

            using var command = new NpgsqlCommand("SELECT c_depid , c_depname FROM t_departmaster;", conn);

            command.CommandType = System.Data.CommandType.Text;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var dep  = new tblDepartment
                {
                    c_depid = Convert.ToInt32(reader["c_depid"]),
                    c_depname = reader["c_depname"].ToString(),    
                };
                depts.Add(dep);
            }
            conn.Close();
            return depts;
        }
        #endregion
    }
}