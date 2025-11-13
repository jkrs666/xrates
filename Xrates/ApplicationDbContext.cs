using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }


    public DbSet<Rate> rates { get; set; }
    public DbSet<Integration> integrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rate>().HasKey(r => r.Id);
        modelBuilder.Entity<Rate>()
        .HasData(
            [
            new Rate { Id=1, Timestamp = new DateTime(2025,1,1, 0,0,0,0, DateTimeKind.Utc), Currency= "EUR", Value= 1.1111M},
            new Rate { Id=2, Timestamp = new DateTime(2025,1,2, 0,0,0,0, DateTimeKind.Utc), Currency= "EUR", Value= 1.2111M},
            new Rate { Id=3, Timestamp = new DateTime(2025,1,3, 0,0,0,0, DateTimeKind.Utc), Currency= "EUR", Value= 1.3111M},
            new Rate { Id=4, Timestamp = new DateTime(2025,1,4, 0,0,0,0, DateTimeKind.Utc), Currency= "EUR", Value= 1.4111M},
        ]
        );

        modelBuilder.Entity<Integration>().HasKey(r => r.Name);
        modelBuilder.Entity<Integration>()
        .HasData(
            [
            new Integration { Name="frankfurter", Url="https://api.frankfurter.dev/v1/latest?base=USD"}
        ]
        );
    }
}

// Store only with USD as base
public class Rate
{
    required public int Id { get; set; }
    required public DateTime Timestamp { get; set; }
    required public String Currency { get; set; }
    required public decimal Value { get; set; }
}

public class Integration
{
    required public string Name { get; set; }
    required public string Url { get; set; }
    //TODO: encrypt
    //public string ApiKey { get; set; }
}
