namespace DatabaseCoreTests;
using Server.Security.Stores;
using Server;
using DatabaseCore.DatabaseComponents;
using Common.Logging;
using Common.POCOs;


public class TestSimulatedShot : DatabaseCoreTestSetup
{
    [Fact]
    public void Test1() => Assert.True(true);

    [Fact]
    public async void testInsertShot()
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

}