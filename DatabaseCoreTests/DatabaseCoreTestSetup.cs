﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Security.Stores;
using Server;
using DatabaseCore.DatabaseComponents;
using Microsoft.AspNetCore.Builder;

namespace DatabaseCoreTests
{
    public class DatabaseCoreTestSetup
    {
        // All database tests must extend this class so the proper setup can occur
        public DatabaseCoreTestSetup()
        {
            // NEED SOMETHING HERE TO RUN SCRIPT TO REBUILD TEST DATABASE

            // Set environment to test environment to ensure the test database is used for the following tests
            Environment.SetEnvironmentVariable("TESTDB_ENV", "true");
            //await FakeBsDatabaseAsync();

        }
    }
    
}
