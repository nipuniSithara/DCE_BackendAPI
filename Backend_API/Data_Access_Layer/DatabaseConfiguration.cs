using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Data_Access_Layer
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; }

        public DatabaseConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
