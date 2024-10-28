using Common.Logging;
using Common.POCOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{

    public async Task<(bool success, List<Shot> shots)> GetShots()
    {
        string connectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        string selectQuery = "SELECT * FROM [Shot]"; // Select all shots

        using var command = new SqlCommand(selectQuery, connection);

        var shots = new List<Shot>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Use while instead of if to handle multiple rows
        {
            // Construct a new Shot object for each entry
            var shot = new Shot(
                shot_id: reader.GetInt32("shot_id"),
                session_id: reader.IsDBNull("session_id") ? (int?)null : reader.GetInt32("session_id"),
                game_id: reader.IsDBNull("game_id") ? (int?)null : reader.GetInt32("game_id"),
                frame_id: reader.IsDBNull("frame_id") ? (int?)null : reader.GetInt32("frame_id"),
                ball_id: reader.IsDBNull("ball_id") ? (int?)null : reader.GetInt32("ball_id"),
                video_id: reader.IsDBNull("video_id") ? (int?)null : reader.GetInt32("video_id"),
                time: reader.GetDateTime("time"),
                shot_number: reader.IsDBNull("shot_number") ? (int?)null : reader.GetInt32("shot_number"),
                shot_number_ot: reader.IsDBNull("shot_number_ot") ? (int?)null : reader.GetInt32("shot_number_ot"),
                lane_number: reader.IsDBNull("lane_number") ? (int?)null : reader.GetInt32("lane_number"),
                pocket_hit: reader.IsDBNull("pocket_hit") ? (int?)null : reader.GetInt32("pocket_hit"),
                count: reader.IsDBNull("count") ? null : reader.GetString("count"),
                pins: reader.IsDBNull("pins") ? null : reader.GetString("pins"),
                ddx: reader.GetFloat("ddx"),
                ddy: reader.GetFloat("ddy"),
                ddz: reader.GetFloat("ddz"),
                x_position: reader.GetFloat("x_position"),
                y_position: reader.GetFloat("y_position"),
                z_position: reader.GetFloat("z_position")
            );

            shots.Add(shot);
        }

        return (shots.Any(), shots);
    }
}
