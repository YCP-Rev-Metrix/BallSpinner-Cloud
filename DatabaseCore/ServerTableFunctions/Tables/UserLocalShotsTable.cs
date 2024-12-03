
using Microsoft.SqlServer.Management.Smo;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void UserLocalShotsTable(Database temp)
    {
        /*
         * SQL used to create this table
         CREATE TABLE User_Local_Shots (
	     userId bigint,
	     ShotName varchar(50),
	     UNIQUE(userId, ShotName),
         )
        Will store list of users local saved shots */
        Console.WriteLine("Creating User_Local_Shots table");

        var userLocalShotTable = new Table(temp, "User_Local_Shots");

        var id = new Column(userLocalShotTable, "userid", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };
        userLocalShotTable.Columns.Add(id);

        var ShotName = new Column(userLocalShotTable, "ShotName", DataType.VarChar(50))
        {
            Nullable = false
        };
        userLocalShotTable.Columns.Add(ShotName);

        if (!temp.Tables.Contains("User_Local_Shots"))
        {
            userLocalShotTable.Create();

            string sql = "ALTER TABLE [User] ADD CONSTRAINT UQ_User_UserId_ShotName UNIQUE (userId, ShotName);";
            temp.ExecuteNonQuery(sql);

            // Foreign key for userID
            var IdKey = new ForeignKey(userLocalShotTable, "SensorType_userID_FK");

            var IdKeyCol = new ForeignKeyColumn(IdKey, "user_id")
            {
                ReferencedColumn = "id"
            };

            IdKey.Columns.Add(IdKeyCol);

            IdKey.ReferencedTable = "User";

            IdKey.Create();

            Console.WriteLine("Success");
        }
    }
}
