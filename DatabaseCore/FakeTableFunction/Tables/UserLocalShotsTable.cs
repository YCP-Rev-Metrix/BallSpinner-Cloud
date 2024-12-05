
using Microsoft.SqlServer.Management.Smo;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Common.POCOs;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcoretest
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

        var userLocalShotTable = new Table(temp, "LocalShots");

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

        if (!temp.Tables.Contains("LocalShots"))
        {
            userLocalShotTable.Create();

            string sql = "ALTER TABLE [LocalShots] ADD CONSTRAINT UQ_User_UserId_ShotName UNIQUE (userid, ShotName);";
            temp.ExecuteNonQuery(sql);

            // Foreign key for userID
            var idKey = new ForeignKey(userLocalShotTable, "LocalShot_userID_FK");

            var idKeyCol = new ForeignKeyColumn(idKey, "userid")
            {
                ReferencedColumn = "id"
            };

            idKey.Columns.Add(idKeyCol);

            idKey.ReferencedTable = "User";

            idKey.Create();

            Console.WriteLine("Success");
        }
    }
}
