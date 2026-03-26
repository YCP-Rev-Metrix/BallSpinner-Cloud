using Common.POCOs;
using Common.POCOs.MobileApp;
using System.Numerics;
using Common.POCOs.PITeam2025;
using System.Security.Cryptography;
using System.Text;

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
    
    public override async Task<bool> UpdateBall(Common.POCOs.MobileApp.Ball ball, string? username)
    {
        return await ServerState.UserStore.UpdateBall(ball, username);
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

        // Fallback for mobile app users stored in combinedDB.[Users].
        (bool combinedSuccess, byte[]? combinedHashedPassword) = await ServerState.UserDatabase.GetCombinedUserValidData(username);
        if (!combinedSuccess || combinedHashedPassword == null) return (false, null);

        // Support commonly observed formats for combined users:
        // 1) pre-hashed bytes sent by client and re-sent as base64 in password field
        // 2) plaintext bytes stored directly (legacy mobile behavior)
        // 3) unsalted SHA256(password) bytes
        bool passwordMatches = false;

        try
        {
            byte[] base64Bytes = Convert.FromBase64String(password ?? string.Empty);
            if (AreByteArraysEqual(base64Bytes, combinedHashedPassword)) passwordMatches = true;
        }
        catch (FormatException)
        {
            // Not base64; continue with other checks.
        }

        if (!passwordMatches)
        {
            byte[] plaintextBytes = Encoding.ASCII.GetBytes(password ?? string.Empty);
            if (AreByteArraysEqual(plaintextBytes, combinedHashedPassword)) passwordMatches = true;
        }

        if (!passwordMatches)
        {
            byte[] sha = SHA256.HashData(Encoding.ASCII.GetBytes(password ?? string.Empty));
            if (AreByteArraysEqual(sha, combinedHashedPassword)) passwordMatches = true;
        }

        return passwordMatches ? (true, new[] { "user" }) : (false, null);
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
        //return await ServerState.UserDatabase.GetShotsByShotname(username, shotname);
        throw new Exception("End point needs to be fixed");
    }

    public override async Task<SimulatedShotList> GetAllShots(string? username)
    {
        //return await ServerState.UserDatabase.GetAllShots(username);
        throw new Exception("End point needs to be fixed");
    }

    public override async Task<bool> AddBalls(Common.POCOs.MobileApp.Ball ball, string? username, int? mobileID = null)
    {
        return await ServerState.UserDatabase.AddBalls(ball, username, mobileID);
    }
    
    public override async Task<List<Common.POCOs.MobileApp.Ball>> GetBalls(string? username, int? mobileID = null)
    {
        return await ServerState.UserDatabase.GetBalls(username, mobileID);
    }

    public override async Task<bool> DeleteBallsByUsername(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteBallsByUsername(username, mobileID);
    public override async Task<bool> DeleteAppEstablishments(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteAppEstablishments(username, mobileID);
    public override async Task<bool> DeleteEventsByUsername(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteEventsByUsername(username, mobileID);
    public override async Task<bool> DeleteAppSessions(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteAppSessions(username, mobileID);
    public override async Task<bool> DeleteAppGames(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteAppGames(username, mobileID);
    public override async Task<bool> DeleteAppFrames(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteAppFrames(username, mobileID);
    public override async Task<bool> DeleteAppShots(string? username, int? mobileID) => await ServerState.UserDatabase.DeleteAppShots(username, mobileID);

    public override async Task<bool> AddFrames(Frame frame, string? username)
    {
        return await ServerState.UserDatabase.AddFrames(frame, username);
    }
    
    public override async Task<List<Frame>> GetFrames(int gameId)
    {
        return await ServerState.UserDatabase.GetFrames(gameId);
    }
    
    public override async Task<bool> AddEvent(Event eventObj, string? username, int? mobileID = null)
    {
        return await ServerState.UserDatabase.AddEvent(eventObj, username, mobileID);
    }
    
    public override async Task<List<Event>> GetEvents(string? username, int? mobileID = null)
    {
        return await ServerState.UserDatabase.GetEvents(username, mobileID);
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

    public override async Task<bool> AddUserCombined(string? firstname, string? lastname, string? username, byte[] hashedPassword, string? phoneNumber, string? email, string? lastLogin, string? hand)
    {
        return await ServerState.UserDatabase.AddUserCombined(firstname, lastname, username, hashedPassword, phoneNumber, email, lastLogin, hand);
    }

    public override async Task<(bool success, List<Common.POCOs.MobileApp.User> users)> GetAppUsers()
    {
        return await ServerState.UserDatabase.GetAppUsers();
    }

    public override async Task<bool> AddEstablishment(Common.POCOs.MobileApp.Establishment establishment)
    {
        return await ServerState.UserDatabase.AddEstablishment(establishment);
    }
    public override async Task<(bool success, List<Common.POCOs.MobileApp.Establishment> establishments)> GetAppEstablishments()
    {
        return await ServerState.UserDatabase.GetAppEstablishments();
    }

    public override async Task<bool> AddShot(Common.POCOs.MobileApp.Shot shot)
    {
        return await ServerState.UserDatabase.AddShot(shot);
    }
    public override async Task<(bool success, List<Common.POCOs.MobileApp.Shot> shots)> GetAppShots()
    {
        return await ServerState.UserDatabase.GetAppShots();

    }
    public override async Task<bool> AddGame(Common.POCOs.MobileApp.Game game)
    {
        return await ServerState.UserDatabase.AddGame(game);
    }
    public override async Task<(bool success, List<Common.POCOs.MobileApp.Game> games)> GetAppGames()
    {
        return await ServerState.UserDatabase.GetAppGames();

    }
    public override async Task<bool> AddSession(Common.POCOs.MobileApp.Session session)
    {
        return await ServerState.UserDatabase.AddSession(session);
    }
    public override async Task<(bool success, List<Common.POCOs.MobileApp.Session> users)> GetAppSessions()
    {
        return await ServerState.UserDatabase.GetAppSessions();

    }
    
    public override async Task<List<int>> AddPiSessions(List<PiSession> sessions)
    {
        return await ServerState.UserDatabase.AddPISessions(sessions);
    }
    
    public override async Task<List<PiSession>> GetAllPiSessions(String rangeStart, String rangeEnd)
    {
        return await ServerState.UserDatabase.GetAllPiSessions(rangeStart, rangeEnd);
    }
    
    public override async Task<List<int>> AddPiShots(List<PiShot> shots)
    {
        return await ServerState.UserDatabase.AddPiShots(shots);
    }
    public override async Task<List<PiShot>> GetAllPiShotsBySession(int sessionId)
    {
        return await ServerState.UserDatabase.GetAllPiShotBySession(sessionId);
    }
    
    public override async Task<List<int>> AddPiDiagnosticScript(List<PiDiagnosticScript> scripts)
    {
        return await ServerState.UserDatabase.AddPiDiagnosticScripts(scripts);
    }
    
    public override async Task<List<PiDiagnosticScript>> GetAllPiDiagnosticScriptsBySession(int sessionId)
    {
        return await ServerState.UserDatabase.GetAllPiDiagnosticScriptBySession(sessionId);
    }
    
    public override async Task<List<int>> AddPiSmartDotData(List<PiSmartDotData> smartDots)
    {
        return await ServerState.UserDatabase.AddPiSmartDotData(smartDots);
    }
    
    public override async Task<List<PiSmartDotData>> GetPiSmartDotDataBySessionId(int smartDotId)
    {
        return await ServerState.UserDatabase.GetAllPiSmartDotDataBySession(smartDotId);
    }
    
    public override async Task<List<int>> AddPiEncoderData(List<PiEncoderData> data)
    {
        return await ServerState.UserDatabase.AddPiEncoderData(data);
    }
    
    public override async Task<List<PiEncoderData>> GetPiEncoderDataBySessionId(int sessionId)
    {
        return await ServerState.UserDatabase.GetAllPiEncoderDataBySession(sessionId);
    }
    public override async Task<List<int>> AddPiHeatData(List<PiHeatData> heatData)
    {
        return await ServerState.UserDatabase.AddPiHeatData(heatData);
    }
    public override async Task<List<PiHeatData>> GetPiHeatDataBySessionId(int sessionId)
    {
        return await ServerState.UserDatabase.GetAllPiHeatDataBySession(sessionId);
    }
}

