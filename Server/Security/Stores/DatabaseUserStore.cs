using Common.POCOs;
using Common.POCOs.MobileApp;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using System.Numerics;
using Common.POCOs.PITeam2025;

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
    
    public override async Task<bool> UpdateBall(Ball ball, string? username)
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
        //return await ServerState.UserDatabase.GetShotsByShotname(username, shotname);
        throw new Exception("End point needs to be fixed");
    }

    public override async Task<SimulatedShotList> GetAllShots(string? username)
    {
        //return await ServerState.UserDatabase.GetAllShots(username);
        throw new Exception("End point needs to be fixed");
    }

    public override async Task<bool> AddBalls(Ball ball, string? username)
    {
        return await ServerState.UserDatabase.AddBalls(ball, username);
    }
    
    public override async Task<List<Ball>> GetBalls(string? username)
    {
        return await ServerState.UserDatabase.GetBalls(username);
    }

    public override async Task<bool> AddFrames(Frame frame, string? username)
    {
        return await ServerState.UserDatabase.AddFrames(frame, username);
    }
    
    public override async Task<List<Frame>> GetFrames(int gameId)
    {
        return await ServerState.UserDatabase.GetFrames(gameId);
    }
    
    public override async Task<bool> AddEvent(Event eventObj, string? username)
    {
        return await ServerState.UserDatabase.AddEvent(eventObj, username);
    }
    
    public override async Task<List<Event>> GetEvents(string? username)
    {
        return await ServerState.UserDatabase.GetEvents(username);
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

    public override async Task<bool> AddUserCombined(string? firstname, string? lastname, string? username, byte[] hashedPassword, string? phone, string? email)
    {
        return await ServerState.UserDatabase.AddUserCombined(firstname, lastname, username, hashedPassword, phone, email);
    }

    public override async Task<(bool success, List<UserTable> users)> GetAppUsers()
    {
        return await ServerState.UserDatabase.GetAppUsers();
    }

    public override async Task<bool> AddEstablishment(string? name, string? lanes, string? type, string? location)
    {
        return await ServerState.UserDatabase.AddEstablishment(name, lanes, type, location);

    }
    public override async Task<(bool success, List<EstablishmentTable> establishments)> GetAppEstablishments()
    {
        return await ServerState.UserDatabase.GetAppEstablishments();
    }

    public override async Task<bool> AddShot(int type, int smartDotId, int sessionId, int ballId, int frameId, int shotNumber, int leaveType, string side, string position, string comment)
    {
        return await ServerState.UserDatabase.AddShot( type,  smartDotId,  sessionId,  ballId,  frameId,  shotNumber,  leaveType, side,  position,  comment);

    }
    public override async Task<(bool success, List<ShotTable> shots)> GetAppShots()
    {
        return await ServerState.UserDatabase.GetAppShots();

    }
    public override async Task<bool> AddGame(string gameNumber, string lanes, int score, int win, int startingLane, int sessionID, int teamResult, int individualResult)
    {
        return await ServerState.UserDatabase.AddGame(gameNumber, lanes, score, win, startingLane, sessionID, teamResult, individualResult);

    }
    public override async Task<(bool success, List<GameTable> games)> GetAppGames()
    {
        return await ServerState.UserDatabase.GetAppGames();

    }
    public override async Task<bool> AddSession(int sessionNumber, int establishmentID, int eventID, int dateTime, string teamOpponent, string individualOpponent, int score, int stats, int teamRecord, int individualRecord)
    {
        return await ServerState.UserDatabase.AddSession( sessionNumber,  establishmentID,  eventID,  dateTime,  teamOpponent,  individualOpponent,  score,  stats,  teamRecord,  individualRecord);

    }
    public override async Task<(bool success, List<SessionTable> users)> GetAppSessions()
    {
        return await ServerState.UserDatabase.GetAppSessions();

    }
    
    public override async Task<List<int>> AddPiSessions(List<PiSession> sessions)
    {
        return await ServerState.UserDatabase.AddPISessions(sessions);
    }
    
    public override async Task<List<PiSession>> GetAllPiSessions(DateTime rangeStart, DateTime rangeEnd)
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
}

