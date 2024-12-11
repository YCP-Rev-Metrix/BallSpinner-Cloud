using System.Numerics;
using Common.POCOs;

namespace Server.Security.Stores;

public class DatabaseUserStore : AbstractUserStore
{
    public override async Task<bool> CreateUser(string? firstname, string? lastname, string? username, string? password, string? email, string? phone_number, string[]? roles = null)
    {
        (byte[] hashed, byte[] salt) = ServerState.SecurityHandler.SaltHashPassword(password);
        string stringRoles = "";
        if (roles != null)
        {
            stringRoles = string.Join(",", roles);
        }
        return await ServerState.UserDatabase.AddUser(firstname, lastname, username, hashed, salt, stringRoles, phone_number, email);
    }

    public override async Task<bool> DeleteUser(string username) => await ServerState.UserDatabase.RemoveUser(username);

    public override async Task<(bool success, string[]? roles)> GetRoles(string username)
    {
        (bool success, string roles) = await ServerState.UserDatabase.GetRoles(username);
        return (success, success ? roles.Split(",") : null);
    }

    public override async Task<(bool success, string[]? roles)> VerifyUser(string? username, string? password)
    {
        (bool success, byte[] salt, string roles, byte[] hashedPassword) result = await ServerState.UserDatabase.GetUserValidData(username);
        if (result.success)
        {
            string[] roles = result.roles.Split(',');
            byte[] hashedPassword = ServerState.SecurityHandler.SaltHashPassword(password, result.salt);
            return hashedPassword.SequenceEqual(result.hashedPassword) ? ((bool success, string[]? roles))(true, roles) : ((bool success, string[]? roles))(false, roles);
        }
        else
        {
            return (false, null);
        }
    }

    public static bool AreByteArraysEqual(byte[] array1, byte[] array2)
    {
        if (array1 == null || array2 == null)
            return false;

        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }
    public override async Task<bool> InsertSampleData(SampleData sampleData)
    {
        return await ServerState.UserDatabase.InsertSampleData(sampleData);
    }

    public override async Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot, string? username)
    {
        return await ServerState.UserDatabase.InsertSimulatedShot(simulatedShot, username);
    }

    public override async Task<int> GetUserId(string? username)
    {
        return await ServerState.UserDatabase.GetUserId(username);
    }
    public override async Task<SimulatedShotList> GetShotsByUsername(string? username)
    {
        return await ServerState.UserDatabase.GetShotsbyUsername(username);
    }

    public override async Task<SimulatedShotList> GetShotsByShotname(string? username, string? shotname)
    {
        return await ServerState.UserDatabase.GetShotsByShotname(username, shotname);
    }

    public override async Task<SimulatedShotList> GetAllShots(string? username)
    {
        return await ServerState.UserDatabase.GetAllShots(username);
    }

    public override async Task<bool> AddBall(Ball ball, string? username)
    {
        return await ServerState.UserDatabase.AddBall(ball, username);
    }

    public override async Task<Arsenal> GetArsenalbyUsername(string? username)
    {
        return await ServerState.UserDatabase.GetArsenalbyUsername(username);
    }

    public override Task<bool> AddSmartDot(SmartDot smartDot, string? username)
    {
        return ServerState.UserDatabase.AddSmartDot(smartDot, username);
    }

    public override Task<bool> DeleteBallByName(string? ballname, string? username)
    {
        return ServerState.UserDatabase.RemoveBall(ballname, username);
    }
    public override Task<bool> DeleteShotByName(string? shotname, string? username)
    {
        return ServerState.UserDatabase.RemoveShot(shotname, username);
    }

}

