using System.Text;
using Common.POCOs;
using Server;

namespace DatabaseCoreTests;

public class TestUser: DatabaseCoreTestSetup
{
    [Fact]
    public void Test1() => Assert.True(true);

    [Fact]
    public async void testAuthValid()
    {
        // Testing with valid user
        string testUserName = "string";
        string testPassword = "string";
        // Validate user credential
        (bool success, string[]? roles) = await ServerState.UserStore.VerifyUser(testUserName, testPassword);
        Assert.True(success);
    }

    [Fact]
    public async void testAuthInvalid()
    {
        // Testing with invalid user
        string invalidUsername = "thisisinvalid";
        string invalidPassword = "thisisinvalid";
        // Validate user credential
        (bool success, string[]? roles) = await ServerState.UserStore.VerifyUser(invalidUsername, invalidPassword);
        Assert.False(success);
    }
    private readonly Random _random = new Random();

        [Fact]
        public async void testRegister()
        {
            var builder = new StringBuilder(5);

            char offset = true ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < 5; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            UserIdentification userIdentification = new UserIdentification("Ryan", "ThaGOD", builder.ToString(),
                "password", "email@email.com", "777-777-7777");
            bool result = await ServerState.UserStore.CreateUser(userIdentification.Firstname,
                userIdentification.Lastname,
                userIdentification.Username,
                userIdentification.Password,
                userIdentification.Email,
                userIdentification.PhoneNumber,
                new string[] { "user" });
            // make sure that the operation was successful
            Assert.True(result);

            (bool success, string[]? roles) =
                await ServerState.UserStore.VerifyUser(userIdentification.Username, userIdentification.Password);
            // make sure that the registered user now exists properly in the database
            Assert.True(success);

        }
        
        [Fact]
        public async void testRegisterWithInvalidUser()
        {
            var builder = new StringBuilder(5);

            char offset = true ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < 5; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            UserIdentification userIdentification = new UserIdentification("Ryan", "ThaGOD", null,
                "password", "email@email.com", "777-777-7777");
            bool result = await ServerState.UserStore.CreateUser(userIdentification.Firstname,
                userIdentification.Lastname,
                userIdentification.Username,
                userIdentification.Password,
                userIdentification.Email,
                userIdentification.PhoneNumber,
                new string[] { "user" });
            // make sure that the operation was successful
            Assert.False(result);
        }
        [Fact]
        public async void TestUserId()
        {
            string TestUserName = "string";
            int result = await ServerState.UserStore.GetUserId(TestUserName);
            Assert.True(result > 0);
        }

}