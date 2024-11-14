using Common.POCOs;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.POCOs;
using Xunit;
namespace DatabaseCoreTests
{
    public class TestRegister : DatabaseCoreTestSetup
    {
        [Fact]
        public async void testRegister()
        {
            UserIdentification userIdentification = new UserIdentification("Ryan", "ThaGOD", "thisisausername", "password", "email@email.com", "777-777-7777");
            bool result = await ServerState.UserStore.CreateUser(userIdentification.Firstname,
                                                                  userIdentification.Lastname,
                                                                  userIdentification.Username,
                                                                  userIdentification.Password,
                                                                  userIdentification.Email,
                                                                  userIdentification.PhoneNumber,
                                                                  new string[] { "user" });
            // make sure that the operation was successful
            Assert.True(result);

            (bool success, string[]? roles) = await ServerState.UserStore.VerifyUser(userIdentification.Username, userIdentification.Password);
            // make sure that the registered user now exists properly in the database
            Assert.True(success);

        }
    }
}
