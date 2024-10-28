using System.Numerics;

namespace Server.Security.Stores;

public abstract class AbstractUserStore
{
    public abstract Task<bool> CreateUser(string firstname, string lastname, string username, string password, string email, string phone_number, string[]? roles = null);
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
    public abstract Task<(bool success, string[]? roles)> VerifyUser(string username, string password);

    public abstract Task<bool> StartSession(//int user_id,
                                                    int? leauge_id,
                                                    int? tournament_id,
                                                    int? practice_id,
                                                    DateTime time,
                                                    string location,
                                                    int total_games,
                                                    int total_frames);
    public abstract Task<bool> StartPractice(int event_id);

    public abstract Task<bool> StartEvent(int user_id, string event_type);

    public abstract Task<bool> StartGame(int session_id, int score);

    public abstract Task<bool> StartFrame(int game_id, int shot_number, int score);


}
