using Common.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task<bool> InsertShot(int? shot_id,
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
        string connectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        string insertQuery = @"
            INSERT INTO [Shot] (
                Shot_id, Session_id, Game_id, Frame_id, Ball_id, Video_id, Time, 
                Shot_number, Shot_number_ot, Lane_Number, Pocket_hit, Count, Pins,
                Ddx, Ddy, Ddz, X_position, Y_position, Z_position
            ) VALUES (
                @Shot_id, @Session_id, @Game_id, @Frame_id, @Ball_id, @Video_id, @Time, 
                @Shot_number, @Shot_number_ot, @Lane_Number, @Pocket_hit, @Count, @Pins,
                @Ddx, @Ddy, @Ddz, @X_position, @Y_position, @Z_position
            )";

        using var command = new SqlCommand(insertQuery, connection);

        // Set parameters, handling nullable values appropriately
        command.Parameters.AddWithValue("@Shot_id", shot_id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Session_id", session_id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Game_id", game_id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Frame_id", frame_id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Ball_id", ball_id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Video_id", video_id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Time", time);
        command.Parameters.AddWithValue("@Shot_number", shot_number ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Shot_number_ot", shot_number_ot ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Lane_Number", lane_number ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Pocket_hit", pocket_hit ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Count", string.IsNullOrEmpty(count) ? (object)DBNull.Value : count);
        command.Parameters.AddWithValue("@Pins", string.IsNullOrEmpty(pins) ? (object)DBNull.Value : pins);
        command.Parameters.AddWithValue("@Ddx", ddx);
        command.Parameters.AddWithValue("@Ddy", ddy);
        command.Parameters.AddWithValue("@Ddz", ddz);
        command.Parameters.AddWithValue("@X_position", x_position);
        command.Parameters.AddWithValue("@Y_position", y_position);
        command.Parameters.AddWithValue("@Z_position", z_position);

        // Execute the query
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
}
