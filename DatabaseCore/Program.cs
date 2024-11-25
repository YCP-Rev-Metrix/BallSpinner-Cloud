namespace DatabaseCore.DatabaseComponents;

internal class Program
{
    private static async Task Main(string[] args)
    {
        
        Console.WriteLine("Select an option:");
        Console.WriteLine("1. Build/Rebuild Prod database");
        Console.WriteLine("2. Build/Rebuild Test database");
        Console.WriteLine("4. Quit");
        var input = Console.ReadLine();
        while (input != "4")
        {
            if (input == "1")
            {
                Console.WriteLine("Building/Rebuilding Prod database");
                await RevMetrixDatabaseAsync();
            }
            else if (input == "2")
            {
                Console.WriteLine("Building/Rebuilding Test database");
                await FakeBsDatabaseAsync();
            }
            
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Build/Rebuild Prod database");
            Console.WriteLine("2. Build/Rebuild Test database");
            Console.WriteLine("4. Quit");
            input = Console.ReadLine();
        }
        Console.WriteLine("Quitting...");
        
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
        var revMetrix = new RevMetrixDb();
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
