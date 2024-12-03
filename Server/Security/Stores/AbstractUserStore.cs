using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;

namespace Server.Security.Stores;

public abstract class AbstractUserStore
{
    public abstract Task<bool> CreateUser(string? firstname, string? lastname, string? username, string? password, 
                                                            string? email, string? phoneNumber, string[]? roles = null);
    public abstract Task<bool> DeleteUser(string username);
    public abstract Task<bool> InsertBall(float weight, string? color);
    public abstract Task<(bool success, string[]? roles)> GetRoles(string username);
    public abstract Task<(bool success, string[]? roles)> VerifyUser(string? username, string? password);

    public abstract Task<bool> InsertSampleData(SampleData sampleData);
    public abstract Task<bool> InsertSimulatedShot(SimulatedShot simulatedShot, string? username);
    public abstract Task<int> GetUserId(string? username);
    public abstract Task<SimulatedShotList> GetShotsByUsername(string? username);
    
    public abstract Task<SimulatedShotList> GetShotsByShotname(string? username, string? shotname);
    public abstract Task<SimulatedShotList> GetAllShots(string? username);

    public abstract Task<bool> AddBall(Ball ball, string? username);
    public abstract Task<Arsenal> GetArsenalbyUsername(string? username);
    
    public abstract Task<bool> AddSmartDot(SmartDot smartDot, string? username);
}
