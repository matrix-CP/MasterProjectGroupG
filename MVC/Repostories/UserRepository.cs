using System;
using System.Data;
using MVC.Models;
using MVC.Repostories;
using Npgsql;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MVC.Repostories
{
    public class UserRepository : CommonRepository, IUserRepository
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

        public UserRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Register(tblUser user)
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO t_usermaster (c_uname, c_uemail, c_password, c_role) VALUES (@uname, @uemail, @password, @role)", conn))
                {
                    cmd.Parameters.AddWithValue("@uname", user.c_uname);
                    cmd.Parameters.AddWithValue("@uemail", user.c_uemail);
                    cmd.Parameters.AddWithValue("@password", user.c_password);
                    cmd.Parameters.AddWithValue("@role", "User");
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();

                var userIdString = user.c_uid.ToString();
                var session = _httpContextAccessor?.HttpContext?.Session;
                
                if (session != null)
                {
                    session.SetString("userid", userIdString);
                    session.SetString("username", user.c_uname);
                    session.SetString("email", user.c_uemail);
                    session.SetInt32("IsAuthenticated", 1);
                }
            }
        }

        public tblUser Login(tblUser user)
        {
            try
            {
                tblUser user1 = new tblUser();
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT c_uid, c_uname, c_uemail, c_password, c_role FROM t_usermaster WHERE c_uemail=@uemail AND c_password=@password;", conn))
                {
                    cmd.Parameters.AddWithValue("@uemail", user.c_uemail);
                    cmd.Parameters.AddWithValue("@password", user.c_password);

                    DataTable data = new DataTable();
                    data.Load(cmd.ExecuteReader());

                    if (data.Rows.Count == 1)
                    {
                        user1.c_uid = Convert.ToInt32(data.Rows[0]["c_uid"]);
                        user1.c_uname = data.Rows[0]["c_uname"].ToString();
                        user1.c_uemail = data.Rows[0]["c_uemail"].ToString();
                        user1.c_password = data.Rows[0]["c_password"].ToString();
                        user1.c_role = data.Rows[0]["c_role"].ToString();

                        var session = _httpContextAccessor?.HttpContext?.Session;
                        
                        if (session != null)
                        {
                            session.SetInt32("userid", Convert.ToInt32(data.Rows[0]["c_uid"]));
                            session.SetString("username", data.Rows[0]["c_uname"].ToString());
                            session.SetString("email", data.Rows[0]["c_uemail"].ToString());
                            session.SetInt32("IsAuthenticated", 1);
                        }
                    }
                }
                return user1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
