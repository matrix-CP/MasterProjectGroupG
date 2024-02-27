using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using MVC.Models; 
using MVC.Repostories; 

namespace MVC.Repostories
{
    public class EmployeeRepository : CommonRepository, IEmployeeRepository
    {

          public bool Register(tblEmployee ct)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO t_empmaster(c_empname, c_empgender, c_dob, c_shift, c_depart, c_img)VALUES (@c_empname, @c_empgender, @c_dob, @c_shift, @c_depart, @c_img);";
             cmd.Parameters.AddWithValue("@c_empname",ct.c_empname);
                          cmd.Parameters.AddWithValue("@c_empgender",ct.c_empgender);
                                       cmd.Parameters.AddWithValue("@c_dob",ct.c_dob.Date);
                                                    cmd.Parameters.AddWithValue("@c_shift",ct.c_shift);
                                                                 cmd.Parameters.AddWithValue("@c_depart",ct.c_depart);
                                                                              cmd.Parameters.AddWithValue("@c_img",ct.c_img);
             
            
            int rowsAffected = cmd.ExecuteNonQuery();
            conn.Close();
            return rowsAffected > 0;
        }

           public void DeleteCity(int id)
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

         public tblEmployee GetCityById(int id)
        {
            tblEmployee city = new tblEmployee();
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_depart, c_img FROM t_empmaster WHERE c_empid = @c_empid";
            cmd.Parameters.AddWithValue("@c_empid", id);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                city.c_empid = Convert.ToInt32(dr["c_empid"]);
                city.c_empname = dr["c_empname"].ToString();
                city.c_empgender = dr["c_empgender"].ToString();
                city.c_dob = DateTime.Parse(dr["c_dob"].ToString());
                city.c_shift = dr["c_shift"].ToString();
                 city.c_img = dr["c_img"].ToString();
                  city.c_depart = Convert.ToInt32(dr["c_depart"]);
            }
            conn.Close();
            return city;
        }

         public void UpdateCity(tblEmployee city)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "UPDATE t_empmaster SET c_empname = @c_empname ,c_empgender=@c_empgender ,c_dob=@c_dob,c_shift=@c_shift,c_img=@c_img,c_depart=@c_depart  WHERE c_empid = @c_empid";
            cmd.Parameters.AddWithValue("@c_empid", city.c_empid);
            cmd.Parameters.AddWithValue("@c_empname", city.c_empname);
             cmd.Parameters.AddWithValue("@c_empgender", city.c_empgender);
            cmd.Parameters.AddWithValue("@c_dob", city.c_dob);
             cmd.Parameters.AddWithValue("@c_shift", city.c_shift);
            cmd.Parameters.AddWithValue("@c_img", city.c_img);
            cmd.Parameters.AddWithValue("@c_depart", city.c_depart);

            cmd.ExecuteNonQuery();
            conn.Close();
        }


         public List<tblEmployee> ViewCity()
        {
            var citys = new List<tblEmployee>();
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
                citys.Add(tblEmployee);
            }
            conn.Close();
            return citys;
        }




    
    public tblEmployee GetOneCity(int c_empid)
    {
        var city = new tblEmployee();
        conn.Open();

        var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandText = "SELECT * FROM t_empmaster WHERE c_empid = @c_empid";

        cmd.Parameters.AddWithValue("@c_empid",c_empid);

        using(var dr = cmd.ExecuteReader())
        {
            while(dr.Read())
            {
               city.c_empid = Convert.ToInt32(dr["c_empid"]);
                city.c_empname = dr["c_empname"].ToString();
                city.c_empgender = dr["c_empgender"].ToString();
                city.c_dob = DateTime.Parse(dr["c_dob"].ToString());
                city.c_shift = dr["c_shift"].ToString();
                 city.c_img = dr["c_img"].ToString();
                  city.c_depart = Convert.ToInt32(dr["c_depart"]);
            }
        }
        conn.Close();
        return city;
    }

     public List<tblEmployee> Viewdept()
        {
            var citys = new List<tblEmployee>();
            conn.Open();

            using var command = new NpgsqlCommand("SELECT c_depid , c_depname FROM t_departmaster;", conn);

            command.CommandType = System.Data.CommandType.Text;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var City  = new tblEmployee
                {
                  c_depid = Convert.ToInt32(reader["c_depid"]),
                 c_depname = reader["c_depname"].ToString(),
                    
                };
                citys.Add(City);
            }
            conn.Close();
            return citys;
        }



        
    }
}