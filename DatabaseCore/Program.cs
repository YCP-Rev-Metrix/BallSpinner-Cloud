namespace DatabaseCore.DatabaseComponents;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await FakeBsDatabaseAsync();
    }

    private static async Task FakeBsDatabaseAsync()
    {
        var revMetrixTest = new RevMetrixBSTest();
        try
        {
            _ = revMetrixTest.NukeAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot Kill because no tables exist in the database.\n{e.Message}");
            throw;
        }
        Console.WriteLine("-----");
        try
        {
            revMetrixTest.CreateTables();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Creating tables failed.\n{e.Message}");
            throw;
        }
    }

    private static async Task BallSpinnerDatabaseAsync()
    {
        var revMetrix = new RevMetrixDB();
        try
        {
            _ = revMetrix.Kill();
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
