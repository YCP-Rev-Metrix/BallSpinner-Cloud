using Common.POCOs;
using Common.POCOs.MobileApp;
using Common.POCOs.PITeam2025;
using Microsoft.SqlServer.Management.Smo;

namespace Server.Security.Stores;

public abstract class AbstractUserStore
{
    //-------------------------Depricated Ball Spinner Application-------------------------------------

    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> CreateUser(string? firstname, string? lastname, string? username, string? password, 
                                                            string? email, string? phoneNumber, string[]? roles = null);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> DeleteUser(string username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<(bool success, string[]? roles)> GetRoles(string username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<(bool success, string[]? roles)> VerifyUser(string? username, string? password);

    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> InsertSampleData(SampleData sampleData);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot, string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<int> GetUserId(string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<SimulatedShotList> GetShotsByUsername(string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<SimulatedShotList> GetShotsByShotname(string? username, string? shotname);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<SimulatedShotList> GetAllShots(string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<Arsenal> GetArsenalbyUsername(string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> AddSmartDot(SmartDot smartDot, string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> DeleteBallByName(string? ballname, string? username);
    
    [Obsolete("Deprecated as part of the Ball Spinner Application. Do not use.")]
    public abstract Task<bool> DeleteShotByName(string? shotname, string? username);

    //-------------------------Phone App-------------------------------------
    public abstract Task<(bool success, List<Common.POCOs.MobileApp.User> users)> GetAppUsers();
    public abstract Task<bool> AddUserCombined(string? firstname, string? lastname, string? username, byte[] hashedPassword, string? phone, string? email, string? lastLogin, string? hand);
    public abstract Task<bool> AddEstablishment(Common.POCOs.MobileApp.Establishment establishment);
    public abstract Task<(bool success, List<Common.POCOs.MobileApp.Establishment> establishments)> GetAppEstablishments();
    public abstract Task<bool> AddShot(Common.POCOs.MobileApp.Shot shot);
    public abstract Task<(bool success, List<Common.POCOs.MobileApp.Shot> shots)> GetAppShots();
    public abstract Task<bool> AddGame(Common.POCOs.MobileApp.Game game);
    public abstract Task<(bool success, List<Common.POCOs.MobileApp.Game> games)> GetAppGames();
    public abstract Task<bool> AddSession(Common.POCOs.MobileApp.Session session);
    public abstract Task<(bool success, List<Common.POCOs.MobileApp.Session> users)> GetAppSessions();
    public abstract Task<bool> AddEvent(Event eventObj, string? username, int? mobileID = null);
    public abstract Task<List<Event>> GetEvents(string? username, int? mobileID = null);
    public abstract Task<List<Common.POCOs.MobileApp.Ball>> GetBalls(string? username, int? mobileID = null);
    public abstract Task<bool> UpdateBall(Common.POCOs.MobileApp.Ball ball, string? username);
    public abstract Task<bool> AddFrames(Frame frame, string? username);
    public abstract Task<List<Frame>> GetFrames(int gameId);
    public abstract Task<bool> AddBalls(Common.POCOs.MobileApp.Ball ball, string? username, int? mobileID = null);
    public abstract Task<bool> DeleteBallsByUsername(string? username, int? mobileID);
    public abstract Task<bool> DeleteAppEstablishments(string? username, int? mobileID);
    public abstract Task<bool> DeleteEventsByUsername(string? username, int? mobileID);
    public abstract Task<bool> DeleteAppSessions(string? username, int? mobileID);
    public abstract Task<bool> DeleteAppGames(string? username, int? mobileID);
    public abstract Task<bool> DeleteAppFrames(string? username, int? mobileID);
    public abstract Task<bool> DeleteAppShots(string? username, int? mobileID);
    public abstract Task<List<int>> AddPiSessions(List<PiSession> sessions);
    public abstract Task<List<PiSession>> GetAllPiSessions(String rangeStart, String rangeEnd);
    public abstract Task<List<int>> AddPiShots(List<PiShot> shots);
    public abstract Task<List<PiShot>> GetAllPiShotsBySession(int sessionId);
    public abstract Task<List<int>> AddPiDiagnosticScript(List<PiDiagnosticScript> scripts);
    public abstract Task<List<PiDiagnosticScript>> GetAllPiDiagnosticScriptsBySession(int sessionId);
    public abstract Task<List<int>> AddPiSmartDotData(List<PiSmartDotData> smartDots);
    public abstract Task<List<PiSmartDotData>> GetPiSmartDotDataBySessionId(int smartDotId);
    public abstract Task<List<int>> AddPiEncoderData(List<PiEncoderData> encoderData);
    public abstract Task<List<PiEncoderData>> GetPiEncoderDataBySessionId(int sessionId);
    public abstract Task<List<int>> AddPiHeatData(List<PiHeatData> heatData);
    public abstract Task<List<PiHeatData>> GetPiHeatDataBySessionId(int sessionId);
}
