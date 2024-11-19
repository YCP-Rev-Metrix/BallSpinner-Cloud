namespace DatabaseCore.DatabaseComponents;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Revmetrix-BS");
        //await RevMetrixDatabaseAsync();
        Console.WriteLine("\n\nRevmetrix-test");
        await FakeBsDatabaseAsync();
    }


    private static async Task FakeBsDatabaseAsync()
    {
        var revMetrixDb = new RevMetrixBSTest();
        try
        { 
            _ = revMetrixDb.NukeAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot Kill because no tables exist in the database.\n{e.Message}");
            throw;
        }
        Console.WriteLine("-----");
        try
        {
            revMetrixDb.CreateTables();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Creating tables failed.\n{e.Message}");
            throw;
        }
    }

    private static async Task RevMetrixDatabaseAsync()
    {
        var revMetrix = new RevMetrixDB();
        try
        {
            _ = revMetrix.NukeAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot Kill because no tables exist in the database.\n{e.Message}");
            throw;
        }

        try
        {
            revMetrix.CreateTables();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Creating tables failed.\n{e.Message}");
            throw;
        }
    }
}
