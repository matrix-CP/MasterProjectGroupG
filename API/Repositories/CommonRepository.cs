using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; 
using API.Repositories;
using Npgsql; 

namespace API.Repositories
{
    public class CommonRepository
    {
   protected NpgsqlConnection conn;

        public CommonRepository()
        {
            IConfiguration myConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            conn = new NpgsqlConnection(myConfig.GetConnectionString("pgconn"));
        }
    }
}