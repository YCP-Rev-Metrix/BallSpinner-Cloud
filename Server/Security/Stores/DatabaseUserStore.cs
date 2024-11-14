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
    public override async Task<bool> InsertBall(float weight, string? color)
    {
        return await ServerState.UserDatabase.Insertball(weight, color);
    }
    public override async Task<bool> InsertSampleData(SampleData sampleData)
    {
        return await ServerState.UserDatabase.InsertSampleData(sampleData);
    }

    public override async Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot)
    {
        return await ServerState.UserDatabase.InsertSimulatedShot(simulatedShot);
    }

}

