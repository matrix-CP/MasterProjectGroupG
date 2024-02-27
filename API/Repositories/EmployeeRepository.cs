using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using API.Models; 
using API.Repositories;

namespace API.Repositories
{
    public class EmployeeRepository : CommonRepository, IEmployeeRepository
    {
         public bool Register(tblEmployee emp)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO t_empmaster(c_empname, c_empgender, c_dob, c_shift, c_depart, c_img)VALUES (@c_empname, @c_empgender, @c_dob, @c_shift, @c_depart, @c_img);";
             cmd.Parameters.AddWithValue("@c_empname",emp.c_empname);
                          cmd.Parameters.AddWithValue("@c_empgender",emp.c_empgender);
                                       cmd.Parameters.AddWithValue("@c_dob",emp.c_dob.Date);
                                                    cmd.Parameters.AddWithValue("@c_shift",emp.c_shift);
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
                emp.c_shift = dr["c_shift"].ToString();
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
             cmd.Parameters.AddWithValue("@c_shift", emp.c_shift);
            cmd.Parameters.AddWithValue("@c_img", emp.c_img);
            cmd.Parameters.AddWithValue("@c_depart", emp.c_depart);

            cmd.ExecuteNonQuery();
            conn.Close();
        }


         public List<tblEmployee> Viewemp()
        {
            var emps = new List<tblEmployee>();
            conn.Open();

            using var command = new NpgsqlCommand("SELECT * FROM t_empmaster;", conn);

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
                
                c_shift = reader["c_shift"].ToString(),
                c_img = reader["c_img"].ToString(),
                c_depart = Convert.ToInt32(reader["c_depart"])
                };
                emps.Add(tblEmployee);
            }
            conn.Close();
            return emps;
        }

     public List<tblEmployee> Viewdept()
        {
            var emps = new List<tblEmployee>();
            conn.Open();

            using var command = new NpgsqlCommand("SELECT c_depid , c_depname FROM t_departmaster;", conn);

            command.CommandType = System.Data.CommandType.Text;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var EMP  = new tblEmployee
                {
                  c_depid = Convert.ToInt32(reader["c_depid"]),
                 c_depname = reader["c_depname"].ToString(),
                    
                };
                emps.Add(EMP);
            }
            conn.Close();
            return emps;
        }



        
    }
}