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
    public abstract Task<bool> InsertSimulatedShot(Shot simulatedShot, string? username);
    public abstract Task<int> GetUserId(string? username);
    public abstract Task<ShotList> GetShotsByUsername(string? username);
    
    public abstract Task<ShotList> GetShotsByShotname(string? username, string? shotname);
    public abstract Task<ShotList> GetAllShots(string? username);

}
