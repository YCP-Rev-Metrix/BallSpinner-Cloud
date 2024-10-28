namespace DatabaseCore.DatabaseComponents;

internal class Program
{
    private static void Main(string[] args)
    {
        var revMetrixDB = new RevMetrixDB();

        _ = revMetrixDB.Kill();
        revMetrixDB.CreateTables();
    }
}
