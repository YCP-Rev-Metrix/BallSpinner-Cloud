using DatabaseCore.DatabaseComponents;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<BallTable> Balls { get; set; }
    public DbSet<UserTable> Users { get; set; }
    public DbSet<FrameTable> Frames { get; set; }
    public DbSet<EventTable> Events { get; set; }
    public DbSet<EstablishmentTable> Establishments { get; set; }
    public DbSet<GameTable> Games { get; set; }
    public DbSet<SessionTable> Sessions { get; set; }
    public DbSet<ShotTable> Shots { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.UseSqlServer(
            "Server=localhost, 1433;User Id=SA; " +
            "Password=Strong1PassMAN!!!;Database=sql_server;TrustServerCertificate=True");
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BallTable>().ToTable("Balls", schema:"combinedDB");
        modelBuilder.Entity<UserTable>().ToTable("Users", schema:"combinedDB");
        modelBuilder.Entity<FrameTable>().ToTable("Frames", schema:"combinedDB");
        modelBuilder.Entity<EventTable>().ToTable("Events", schema:"combinedDB");
        modelBuilder.Entity<EstablishmentTable>().ToTable("Establishments", schema: "combinedDB");
        modelBuilder.Entity<GameTable>().ToTable("Games", schema: "combinedDB");
        modelBuilder.Entity<SessionTable>().ToTable("Sessions", schema: "combinedDB");
        modelBuilder.Entity<ShotTable>().ToTable("Shots", schema: "combinedDB");
    }
}