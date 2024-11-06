using Common.Logging;
using Common.POCOs;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.Numerics;

namespace DatabaseCore.DatabaseComponents;

public partial class RevMetrixDB
{
    private void SmartDotTable(Database temp)
    {
        // SmartDot Table
        {
            var SmartDotTable = new Table(temp, "SmartDot");

            var smartdot_id = new Column(SmartDotTable, "smartdot_id", DataType.BigInt)
            {
                IdentityIncrement = 1,
                Nullable = false,
                IdentitySeed = 1,
                Identity = true
            };
            SmartDotTable.Columns.Add(smartdot_id);

            var mac_address = new Column(SmartDotTable, "mac_address", DataType.VarChar(48))
            {
                Nullable = false
            };
            SmartDotTable.Columns.Add(mac_address);

            var name = new Column(SmartDotTable, "name", DataType.VarChar(100))
            {
                Nullable = false
            };
            SmartDotTable.Columns.Add(name);

            if (!temp.Tables.Contains("SmartDot"))
            {
                SmartDotTable.Create();

                string sql = "ALTER TABLE [SmartDot] ADD CONSTRAINT SmartDotConnections_PK PRIMARY KEY (smartdot_id);";
                temp.ExecuteNonQuery(sql);
            }
        }
    }
}