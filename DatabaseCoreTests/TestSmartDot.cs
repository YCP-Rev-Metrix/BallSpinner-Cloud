using System.Text;
using Common.POCOs;
using Server;

namespace DatabaseCoreTests;

public class TestSmartDot: DatabaseCoreTestSetup
{
    private readonly Random _random = new Random();

    [Fact]
    public async void TestInsertSmartDot()
    {
        var builder = new StringBuilder(5);

        char offset = true ? 'a' : 'A';
        const int lettersOffset = 26; 

        for (var i = 0; i < 5; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }
        
        SmartDot smartDot = new SmartDot
        {
            Name = builder.ToString(),
            MacAddress = "1a:2b:3c:4d:5e:6f"
        };
        string testUserName = "string";

        bool success = await ServerState.UserStore.AddSmartDot(smartDot, testUserName);
        Assert.True(success);
    }
    
    [Fact]
    public async void TestInsertSmartDotWithFalseUser()
    {
        var builder = new StringBuilder(5);

        char offset = true ? 'a' : 'A';
        const int lettersOffset = 26; 

        for (var i = 0; i < 5; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }
        
        SmartDot smartDot = new SmartDot
        {
            Name = builder.ToString(),
            MacAddress = "1a:2b:3c:4d:5e:6f"
        };
        string testUserName = "nouser";

        bool success = await ServerState.UserStore.AddSmartDot(smartDot, testUserName);
        Assert.False(success);
    }
}