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
        private readonly IConfiguration _configuration;

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
            _configuration = configuration; // Assign IConfiguration in the constructor
            _httpContextAccessor = httpContextAccessor;
        }

        public List<tblEmployee> FetchEmployeeDetails()
        {
            List<tblEmployee> employees = new List<tblEmployee>(); // Corrected the variable name to plural 'employees'
            
            using (var conn = new NpgsqlConnection(_configuration.GetConnectionString("YourConnectionString"))) // Use IConfiguration to get the connection string
            {
                try
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_empgender, e.c_dob, e.c_shift, e.c_depart, e.c_img, d.c_depname FROM t_empmaster e INNER JOIN t_departmaster d ON e.c_depart = d.c_depart WHERE c_uid = @c_uid", conn))
                    {
                        command.Parameters.AddWithValue("@c_uid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read()) // Use while loop to read all records
                            {
                                tblEmployee emp = new tblEmployee(); // Rename the variable to 'emp'
                                emp.c_empid = Convert.ToInt32(reader["c_empid"]);
                                emp.c_empname = reader["c_empname"].ToString();
                                emp.c_empgender = reader["c_empgender"].ToString();
                                emp.c_dob = DateTime.Parse(reader["c_dob"].ToString()).Date;
                                emp.c_shift = reader["c_shift"].ToString().Split(',').ToList();
                                emp.c_depart = Convert.ToInt32(reader["c_depart"]);
                                emp.depname = reader["c_depname"].ToString();
                                emp.c_img = reader["c_img"].ToString();
                                employees.Add(emp); // Add the employee to the list
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            return employees;
        }

        
    }
}