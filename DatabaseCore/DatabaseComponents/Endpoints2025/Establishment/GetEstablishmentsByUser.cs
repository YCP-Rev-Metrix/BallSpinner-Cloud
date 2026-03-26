using Common.POCOs.MobileApp;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<List<Establishment>> GetEstablishmentsByUser(string? username, int? mobileID = null)
    {
        int userId = mobileID.HasValue && mobileID.Value > 0
            ? await GetUserId(username, mobileID)
            : await GetUserId(username);
        if (userId <= 0) return new List<Establishment>();

        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // Establishments table does not have a UserId column; scope via Sessions -> Events ownership.
        const string selectQuery = @"
            SELECT DISTINCT est.ID, est.Name, est.Lanes, est.Type, est.Location, est.MobileID
            FROM [combinedDB].[Establishments] est
            INNER JOIN [combinedDB].[Sessions] s ON s.EstablishmentID = est.ID
            INNER JOIN [combinedDB].[Events] e ON e.ID = s.EventID
            WHERE e.UserId = @UserId;";

        using var command = new SqlCommand(selectQuery, connection);
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

        var establishments = new List<Establishment>();
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            establishments.Add(new Establishment
            {
                Id = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null,
                MobileID = reader["MobileID"] != DBNull.Value ? Convert.ToInt32(reader["MobileID"]) : null,
                Name = reader["Name"] as string,
                Lanes = reader["Lanes"] as string,
                Type = reader["Type"] as string,
                Location = reader["Location"] as string
            });
        }

        return establishments;
    }
}

