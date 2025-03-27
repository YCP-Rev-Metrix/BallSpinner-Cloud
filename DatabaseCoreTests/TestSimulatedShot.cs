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

        // Testing with valid user with invalid ball
        Ball ball = new Ball(builder.ToString(), 2, 2, "Pancake");
        ShotInfo shotInfo = new ShotInfo
        {
            Name = builder.ToString(),
            BezierInitPoint = new Coordinate(0, 0),
            BezierInflectionPoint = new Coordinate(0, 0),
            BezierFinalPoint = new Coordinate(0, 0),
            TimeStep = 0.010,
            Comments = "Test"
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
            ball = ball,
        };
        
        // Should return false, as this user does not have this ball in the database
        bool success = await ServerState.UserStore.InsertSimulatedShot(simulatedShot, TestUsername);
        Assert.False(success);

        bool ballInserted = await ServerState.UserStore.AddBall(ball, TestUsername);

        Assert.True(ballInserted);

        // Validate user credential
        bool trueShotInsert = await ServerState.UserStore.InsertSimulatedShot(simulatedShot, TestUsername);
        Assert.True(trueShotInsert);
    }

    [Fact]
    public async void TestInsertShotWithFalseUser()
    {
        // Testing with valid user
        ShotInfo shotInfo = new ShotInfo
        {
            Name = "simulatedshottest",
            BezierInitPoint = new Coordinate(0,0),
            BezierInflectionPoint = new Coordinate(0, 0),
            BezierFinalPoint = new Coordinate(0, 0),
            TimeStep = 0.010,
            Comments = "Test"
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
        Ball ball = new Ball(builder.ToString(), 2, 2, "Pancake");
        bool ballInserted = await ServerState.UserStore.AddBall(ball, TestUsername);
        ShotInfo shotInfo = new ShotInfo
        {
            Name = testShotName,
            BezierInitPoint = new Coordinate(0, 0),
            BezierInflectionPoint = new Coordinate(2, 3),
            BezierFinalPoint = new Coordinate(1, 2),
            TimeStep = 0.010,
            Comments = "Test"
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
            ball = ball
        };
        bool success = await ServerState.UserStore.InsertSimulatedShot(simulatedShot, TestUsername);
        Assert.True(success);

        // Validate user credential
        success = await ServerState.UserStore.DeleteShotByName(testShotName, TestUsername);
        Assert.True(success);
    }

}