using System.Text;
using Common.POCOs;
using Server;

namespace DatabaseCoreTests;

public class TestArsenal: DatabaseCoreTestSetup
{
    private readonly Random _random = new Random();


    public string RandomString(int length)
    {
        var builder = new StringBuilder(length);

        char offset = true ? 'a' : 'A';
        const int lettersOffset = 26; 

        for (var i = 0; i < 5; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }
        return builder.ToString();
    }
    
    [Fact]
    public async void TestInsertBall()
    {
        Ball ball = new Ball
        {
            Name = RandomString(5),
            Diameter = 12,
            Weight = 12.3,
            CoreType = "Symmetrical"
        };
        string testUserName = "string";

        bool success = await ServerState.UserStore.AddBall(ball, testUserName);
        Assert.True(success);
    }

    [Fact]
    public async void TestInsertBallWithInvalidUser()
    {
        Ball ball = new Ball
        {
            Name = RandomString(5),
            Diameter = 12,
            Weight = 12.3,
            CoreType = "Symmetrical"
        };
        string testUserName = "nouser";

        bool success = await ServerState.UserStore.AddBall(ball, testUserName);
        Assert.False(success);
    }
    
    [Fact]
    public async void TestGetArsenal()
    {
        string testUserName = "string";
        Arsenal arsenal = await ServerState.UserStore.GetArsenalbyUsername(testUserName);
        Assert.True(arsenal!=null);
    }
    
    [Fact]
    public async void TestGetArsenalWithInvalidUser()
    {
        string testUserName = "nouser";
        Arsenal arsenal = await ServerState.UserStore.GetArsenalbyUsername(testUserName);
        Assert.True(arsenal!=null);
    }

    [Fact]
    public async void TestDeleteBall()
    {
        string testUserName = "string";
        
        bool success = await ServerState.UserStore.DeleteBallByName("string", testUserName);
        Assert.True(success);
    }

}