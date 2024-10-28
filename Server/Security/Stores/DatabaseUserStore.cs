using System.Numerics;

namespace Server.Security.Stores;

public class DatabaseUserStore : AbstractUserStore
{
    public override async Task<bool> CreateUser(string firstname, string lastname, string username, string password, string email, string phone_number, string[]? roles = null)
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

    public override async Task<(bool success, string[]? roles)> VerifyUser(string username, string password)
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

    public override async Task<bool> InsertShot(int? shot_id,
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
                                            float z_position)
    {
        return await ServerState.UserDatabase.InsertShot(shot_id, session_id, game_id, frame_id, ball_id, video_id, time, shot_number, shot_number_ot, lane_number, pocket_hit, count, pins, ddx, ddy, ddz, x_position, y_position, z_position);
    }


    public override async Task<bool> InsertBall(float weight, string? color)
    {
        return await ServerState.UserDatabase.Insertball(weight, color);
    }

    public override async Task<bool> StartSession(//int user_id,
                                                    int? leauge_id, int? tournament_id, int? practice_id, DateTime time, string location, int total_games, int total_frames)
    {
        return await ServerState.UserDatabase.Startsession(//int user_id,
                                                           leauge_id, tournament_id, practice_id, time, location, total_games, total_frames);
    }

    public override async Task<bool> StartPractice(int event_id)
    {
        return await ServerState.UserDatabase.Startpractice(event_id);
    }

    public override async Task<bool> StartEvent(int user_id, string event_type)
    {
        return await ServerState.UserDatabase.Startevent(user_id, event_type);
    }

    public override async Task<bool> StartGame(int session_id, int score)
    {
        return await ServerState.UserDatabase.Startgame(session_id, score);
    }
    public override async Task<bool> StartFrame(int game_id, int shot_number, int score)
    {
        return await ServerState.UserDatabase.Startframe(game_id, shot_number, score);
    }
}

