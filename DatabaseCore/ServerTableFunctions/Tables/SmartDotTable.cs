using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace DatabaseCore.DatabaseComponents;

public partial class Dbcore
{
    private void SmartDotTable(Database temp)
    {
        Console.WriteLine("Creating SmartDotTable");
        var smartDotTable = new Table(temp, "SmartDot");
        
        var smartdotid = new Column(smartDotTable, "smartdot_id", DataType.BigInt)
        {
            IdentityIncrement = 1,
            Nullable = false,
            IdentitySeed = 1,
            Identity = true
        };

        smartDotTable.Columns.Add(smartdotid);

        var name = new Column(smartDotTable, "name", DataType.VarChar(100))
        {
            Nullable = false
        };

        smartDotTable.Columns.Add(name);

        var weight = new Column(smartDotTable, "address", DataType.VarChar(48))
        {
            Nullable = false
        };

        smartDotTable.Columns.Add(weight);


        if (!temp.Tables.Contains("SmartDot"))
        {
            smartDotTable.Create();

            string sql = "ALTER TABLE [SmartDot] ADD CONSTRAINT SmartDot_PK PRIMARY KEY (smartdot_id);";
            temp.ExecuteNonQuery(sql);
            
            sql = "ALTER TABLE [SmartDot] ADD CONSTRAINT SmartDot_UNIQUE UNIQUE (name);";
            temp.ExecuteNonQuery(sql);

            Console.WriteLine("Success");
        }
        
    }
}