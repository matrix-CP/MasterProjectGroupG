using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace MVC.Repostories
{
    public class CommonRepository
    {
        protected NpgsqlConnection conn;


        public CommonRepository()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            conn = new NpgsqlConnection(config.GetConnectionString("pgconn"));

        }
    }
}