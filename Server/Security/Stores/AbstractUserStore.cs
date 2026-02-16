using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Common.POCOs.MobileApp;
using Common.POCOs.PITeam2025;
using Microsoft.SqlServer.Management.Smo;

namespace Server.Security.Stores;

public abstract class AbstractUserStore
{
    //-------------------------Depricated Ball Spinner Application-------------------------------------

    public abstract Task<bool> CreateUser(string? firstname, string? lastname, string? username, string? password, 
                                                            string? email, string? phoneNumber, string[]? roles = null);
    public abstract Task<bool> DeleteUser(string username);
    public abstract Task<(bool success, string[]? roles)> GetRoles(string username);
    public abstract Task<(bool success, string[]? roles)> VerifyUser(string? username, string? password);

    public abstract Task<bool> InsertSampleData(SampleData sampleData);
    public abstract Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot, string? username);
    public abstract Task<int> GetUserId(string? username);
    public abstract Task<SimulatedShotList> GetShotsByUsername(string? username);
    
    public abstract Task<SimulatedShotList> GetShotsByShotname(string? username, string? shotname);
    public abstract Task<SimulatedShotList> GetAllShots(string? username);
    
    public abstract Task<Arsenal> GetArsenalbyUsername(string? username);
    
    public abstract Task<bool> AddSmartDot(SmartDot smartDot, string? username);
    public abstract Task<bool> DeleteBallByName(string? ballname, string? username);
    public abstract Task<bool> DeleteShotByName(string? shotname, string? username);

    //-------------------------Phone App-------------------------------------
    public abstract Task<(bool success, List<UserTable> users)> GetAppUsers();
    public abstract Task<bool> AddUserCombined(string? firstname, string? lastname, string? username, byte[] hashedPassword, string? phone, string? email, string? lastLogin, string? hand);
    public abstract Task<bool> AddEstablishment(string? name, string? lanes, string? type, string? location);
    public abstract Task<(bool success, List<EstablishmentTable> establishments)> GetAppEstablishments();
    public abstract Task<bool> AddShot(int type, int smartDotId, int sessionId, int ballId, int frameId, int shotNumber, int leaveType, string side, string position, string comment);
    public abstract Task<(bool success, List<ShotTable> shots)> GetAppShots();
    public abstract Task<bool> AddGame(string gameNumber, string lanes, int score, int win, int startingLane, int sessionID, int teamResult, int individualResult);
    public abstract Task<(bool success, List<GameTable> games)> GetAppGames();
    public abstract Task<bool> AddSession(int sessionNumber, int establishmentID, int eventID, int dateTime, string teamOpponent, string individualOpponent, int score, int stats, int teamRecord, int individualRecord);
    public abstract Task<(bool success, List<SessionTable> users)> GetAppSessions();
    public abstract Task<bool> AddEvent(Event eventObj, string? username);
    public abstract Task<List<Event>> GetEvents(string? username);
    public abstract Task<List<Ball>> GetBalls(string? username);
    public abstract Task<bool> UpdateBall(Ball ball, string? username);
    public abstract Task<bool> AddFrames(Frame frame, string? username);
    public abstract Task<List<Frame>> GetFrames(int gameId);
    public abstract Task<bool> AddBalls(Ball ball, string? username);
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
