using System.Numerics;
using Common.POCOs;

namespace Server.Security.Stores;

public abstract class AbstractUserStore
{
    public abstract Task<bool> CreateUser(string? firstname, string? lastname, string? username, string? password, 
                                                            string? email, string? phoneNumber, string[]? roles = null);
    public abstract Task<bool> DeleteUser(string username);
    public abstract Task<bool> InsertShot(int? shot_id,
                                      int? session_id,
                                      int? game_id,
                                      int? frame_id,
                                      int? ball_id,
                                      int? video_id,
                                      DateTime time,
                                      int? shot_number,
                                      int? shot_number_ot,
                                      int? lane_number,
                                      int? pocket_hit,
                                      string? count,
                                      string? pins,
                                      float ddx,
                                      float ddy,
                                      float ddz,
                                      float x_position,
                                      float y_position,
                                      float z_position);

    public abstract Task<bool> InsertBall(float weight, string? color);
    public abstract Task<(bool success, string[]? roles)> GetRoles(string username);
    public abstract Task<(bool success, string[]? roles)> VerifyUser(string? username, string? password);

    public abstract Task<bool> InsertSampleData(SampleData sampleData);
}
