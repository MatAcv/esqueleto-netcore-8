﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SqlConfiguration
    {
        public SqlConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
                
        }
        public string ConnectionString { get; set; }
    }
}
