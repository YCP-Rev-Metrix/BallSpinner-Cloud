using Microsoft.Data.SqlClient;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDb
{
    public async Task<bool> DeleteOrphanedAppData()
    {
        ConnectionString = Environment.GetEnvironmentVariable("SERVERDB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(ConnectionString)) return false;

        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            const string deleteOrphanedSessions = @"
                DELETE s
                FROM combinedDB.[Sessions] s
                LEFT JOIN combinedDB.[Events] e ON e.Id = s.EventID
                WHERE e.Id IS NULL;";

            const string deleteOrphanedGames = @"
                DELETE g
                FROM combinedDB.[Games] g
                LEFT JOIN combinedDB.[Sessions] s ON s.ID = g.SessionID
                WHERE s.ID IS NULL;";

            const string deleteOrphanedFrames = @"
                DELETE f
                FROM combinedDB.[Frames] f
                LEFT JOIN combinedDB.[Games] g ON g.ID = f.GameId
                WHERE g.ID IS NULL;";

            const string deleteOrphanedShots = @"
                DELETE s
                FROM combinedDB.[Shots] s
                LEFT JOIN combinedDB.[Sessions] sess ON sess.ID = s.SessionID
                LEFT JOIN combinedDB.[Frames] f ON f.Id = s.FrameID
                LEFT JOIN combinedDB.[Balls] b ON b.Id = s.BallID
                WHERE sess.ID IS NULL OR f.Id IS NULL OR b.Id IS NULL;";

            // Root to leaf works best when events are already gone.
            using (var cmd = new SqlCommand(deleteOrphanedSessions, connection, (SqlTransaction)transaction))
                await cmd.ExecuteNonQueryAsync();
            using (var cmd = new SqlCommand(deleteOrphanedGames, connection, (SqlTransaction)transaction))
                await cmd.ExecuteNonQueryAsync();
            using (var cmd = new SqlCommand(deleteOrphanedFrames, connection, (SqlTransaction)transaction))
                await cmd.ExecuteNonQueryAsync();
            using (var cmd = new SqlCommand(deleteOrphanedShots, connection, (SqlTransaction)transaction))
                await cmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}

