using System;
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
        /// <summary>
        /// Represents the username of the valid test user
        /// </summary>
        public string TestUsername;
        /// <summary>
        /// Represents the password of the valid test user
        /// </summary>
        public string TestPassword;

        // All database tests must extend this class so the proper setup can occur
        // Make sure that the test.runsettings file is set for this module to access environmental variables
        public DatabaseCoreTestSetup()
        {
            Random _random = new Random();
            // Create random user
            StringBuilder TestUserBuilder = new StringBuilder();
            StringBuilder TestUserPasswordBuilder = new StringBuilder();

            char offset = true ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < 5; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                TestUserBuilder.Append(@char);
                TestUserPasswordBuilder.Append(@char);
            }

            TestUsername = TestUserBuilder.ToString();
            TestPassword = TestUserPasswordBuilder.ToString();

            // Insert main authentic user into database
            CreateTestUser();
            Thread.Sleep(3000);
        }
        /// <summary>
        /// Creates a user that will be used in test cases to test valid user operations
        /// </summary>
        public async void CreateTestUser()
        {
            // Testing with valid user
            await ServerState.UserStore.CreateUser("string", "string", TestUsername, TestPassword, "email@email.com", "777-111-1111");
        }
    }
    
}
