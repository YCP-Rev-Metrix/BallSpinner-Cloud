using Common.POCOs;
using Common.POCOs.MobileApp;
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

    public abstract Task<bool> AddBalls(Ball ball, string? username);

    public abstract Task<List<Ball>> GetBalls(string? username);
    public abstract Task<bool> UpdateBall(Ball ball, string? username);
    
    public abstract Task<bool> AddFrames(Frame frame, string? username);
    
    public abstract Task<List<Frame>> GetFrames(int gameId);
    public abstract Task<Arsenal> GetArsenalbyUsername(string? username);
    
    public abstract Task<bool> AddEvent(Event eventObj, string? username);
    
    public abstract Task<List<Event>> GetEvents(string? username);
    
    public abstract Task<bool> AddSmartDot(SmartDot smartDot, string? username);
    public abstract Task<bool> DeleteBallByName(string? ballname, string? username);
    public abstract Task<bool> DeleteShotByName(string? shotname, string? username);

    //-------------------------Phone App-------------------------------------

    public abstract Task<bool> AddUserCombined(string? firstname, string? lastname, string? username, byte[] hashedPassword, string? phone, string? email);
}
