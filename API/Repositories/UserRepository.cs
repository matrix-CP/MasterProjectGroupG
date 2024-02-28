using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Npgsql;

namespace API.Repositories
{
    public class UserRepository : CommonRepository, IUserRepository
    {
        public void Register(tblUser user)
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("INSERT INTO public.t_usermaster (c_uname, c_uemail, c_password, c_role) VALUES (@uname, @email, @password, @role);",conn))
            {
                cmd.Parameters.AddWithValue("@uname", user.c_uname);
                cmd.Parameters.AddWithValue("@email", user.c_uemail);
                cmd.Parameters.AddWithValue("@password", user.c_password);
                cmd.Parameters.AddWithValue("@role", "user");
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public tblUser Login(tblUser user)
        {
            tblUser user1 = null;
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT c_uid, c_uname, c_uemail, c_password, c_role FROM public.t_usermaster WHERE c_uemail=@uemail AND c_password=@password;",conn))
            {
                cmd.Parameters.AddWithValue("@uemail", user.c_uemail);
                cmd.Parameters.AddWithValue("@password", user.c_password);
                
                DataTable data = new DataTable();
                data.Load(cmd.ExecuteReader());

                if (data.Rows.Count == 1)
                {
                    user1=new tblUser();
                    user1.c_uid = Convert.ToInt32(data.Rows[0]["c_uid"]);
                    user1.c_uname = data.Rows[0]["c_uname"].ToString();
                    user1.c_uemail = data.Rows[0]["c_uemail"].ToString();
                    user1.c_password = data.Rows[0]["c_password"].ToString();
                    user1.c_role = data.Rows[0]["c_role"].ToString();
                }
            }
            conn.Close();
            return user1;
        }
    }
}