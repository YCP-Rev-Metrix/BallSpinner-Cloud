
using Microsoft.SqlServer.Management.Smo;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

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
        // NOT IMPLEMENTED YET!!!!!!!!!!!!!!!!!!!
    }
}
