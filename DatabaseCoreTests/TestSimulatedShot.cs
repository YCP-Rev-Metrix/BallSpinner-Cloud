using System.Text;

namespace DatabaseCoreTests;
using Server.Security.Stores;
using Server;
using DatabaseCore.DatabaseComponents;
using Common.Logging;
using Common.POCOs;


public class TestSimulatedShot : DatabaseCoreTestSetup
{
    private readonly Random _random = new Random();
    [Fact]
    public void Test1() => Assert.True(true);

    [Fact]
    public async void TestInsertShot()
    {
        var builder = new StringBuilder(5);

        char offset = true ? 'a' : 'A';
        const int lettersOffset = 26; 

        for (var i = 0; i < 5; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }
        
        // Testing with valid user
        ShotInfo shotInfo = new ShotInfo
        {
            Name = builder.ToString(),
            Speed = 99,
            Angle = 99,
            Position = 99,
            Frequency = 99
        };
        List<SampleData?> data = new List<SampleData?>
        {
            new SampleData
            {
                Type = "1",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "2",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "3",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "3",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            }
        };

        SimulatedShot simulatedShot = new SimulatedShot
        {
            shotinfo = shotInfo,
            data = data,
        };
        
        string testUserName = "string";
        // Validate user credential
        bool success = await ServerState.UserStore.InsertSimulatedShot(simulatedShot, testUserName);
        Assert.True(success);
    }

    [Fact]
    public async void TestInsertShotWithFalseUser()
    {
        // Testing with valid user
        ShotInfo shotInfo = new ShotInfo
        {
            Name = "simulatedshottest",
            Speed = 99,
            Angle = 99,
            Position = 99,
            Frequency = 99
        };
        List<SampleData?> data = new List<SampleData?>
        {
            new SampleData
            {
                Type = "1",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "2",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "3",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "3",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            }
        };

        SimulatedShot simulatedShot = new SimulatedShot
        {
            shotinfo = shotInfo,
            data = data,
        };
        
        string testUserName = "nouser";
        // Validate user credential
        bool success = await ServerState.UserStore.InsertSimulatedShot(simulatedShot, testUserName);
        Assert.False(success);
    }
    [Fact]
    public async void TestRemoveShotWithFalseUser()
    {
        string testShotName = "simulatedshottest"; 
        string testUserName = "nouser";
        // Validate user credential
        bool success = await ServerState.UserStore.DeleteShotByName(testShotName, testUserName);
        Assert.False(success);
    }

    
    [Fact]
    public async void TestRemoveShot()
    {
        var builder = new StringBuilder(5);

        char offset = true ? 'a' : 'A';
        const int lettersOffset = 26;

        for (var i = 0; i < 5; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        string testShotName = builder.ToString(); 
        string testUserName = "string";
        ShotInfo shotInfo = new ShotInfo
        {
            Name = testShotName,
            Speed = 99,
            Angle = 99,
            Position = 99,
            Frequency = 99
        };
        List<SampleData?> data = new List<SampleData?>
        {
            new SampleData
            {
                Type = "1",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "2",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "3",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            },
            new SampleData
            {
                Type = "3",
                Logtime = 1234,
                Count = 99,
                X = 12,
                Y = 12,
                Z = 12,
            }
        };

        SimulatedShot simulatedShot = new SimulatedShot
        {
            shotinfo = shotInfo,
            data = data,
        };
        bool success = await ServerState.UserStore.InsertSimulatedShot(simulatedShot, testUserName);
        Assert.True(success);

        // Validate user credential
        success = await ServerState.UserStore.DeleteShotByName(testShotName, testUserName);
        Assert.True(success);
    }

}