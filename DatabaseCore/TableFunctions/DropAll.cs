using Common.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    public async Task Kill()
    {
        using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        // This will need to be adjusted to see if constraints are being reset in order.
        // As well as the order of Dropping the table 

        // Get Rid of The Key Constraints for Shot
        string noConstraint = "Use [revmetrix-bs] ALTER TABLE [Shot] NOCHECK CONSTRAINT all";
        using var command1 = new SqlCommand(noConstraint, connection);
        _ = command1.ExecuteNonQuery();

        // Dropping the Shot Table
        string dropShot = "DROP TABLE [Shot]";
        using var command2 = new SqlCommand(dropShot, connection);
        _ = command2.ExecuteNonQuery();

        // Dropping the Video Table
        string dropVideo = "DROP TABLE [Video]";
        using var command3 = new SqlCommand(dropVideo, connection);
        _ = command3.ExecuteNonQuery();

        // Dropping the Ball Table
        string dropBall = "DROP TABLE [Ball]";
        using var command4 = new SqlCommand(dropBall, connection);
        _ = command4.ExecuteNonQuery();

        // Dropping the Frame Table
        string dropFrame = "DROP TABLE [Frame]";
        using var command5 = new SqlCommand(dropFrame, connection);
        _ = command5.ExecuteNonQuery();

        // Dropping the Game Table
        string dropGame = "DROP TABLE [Game]";
        using var command6 = new SqlCommand(dropGame, connection);
        _ = command6.ExecuteNonQuery();

        // Dropping the Session Table
        string dropSession = "DROP TABLE [Session]";
        using var command7 = new SqlCommand(dropSession, connection);
        _ = command7.ExecuteNonQuery();

        // Dropping the League Table
        string dropLeague = "DROP TABLE [League]";
        using var command8 = new SqlCommand(dropLeague, connection);
        _ = command8.ExecuteNonQuery();

        // Dropping the Tournament Table
        string dropTournament = "DROP TABLE [Tournament]";
        using var command9 = new SqlCommand(dropTournament, connection);
        _ = command9.ExecuteNonQuery();

        // Dropping the Practice Table
        string dropPractice = "DROP TABLE [Practice]";
        using var command10 = new SqlCommand(dropPractice, connection);
        _ = command10.ExecuteNonQuery();

        // Dropping the Event Table 
        string dropEvent = "DROP TABLE [Event]";
        using var command11 = new SqlCommand(dropEvent, connection);
        _ = command11.ExecuteNonQuery();

        // Dropping the RefreshToken Table
        string dropRefreshToken = "DROP TABLE [RefreshToken]";
        using var command12 = new SqlCommand(dropRefreshToken, connection);
        _ = command12.ExecuteNonQuery();

        // Droping the User Table
        string dropUser = "DROP TABLE [User]";
        using var command13 = new SqlCommand(dropUser, connection);
        _ = command13.ExecuteNonQuery();
    }
}