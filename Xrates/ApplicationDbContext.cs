using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }


    public DbSet<Rate> Rates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rate>().HasKey(r => r.Id);
        modelBuilder.Entity<Rate>()
        .HasData(
            [
            new Rate { Id=1, UnixTs = new DateTime(2025,1,1, 0,0,0,0, DateTimeKind.Utc), Base = Currency.USD, Quote= Currency.EUR, Value= 1.1111M},
            new Rate { Id=2, UnixTs = new DateTime(2025,1,2, 0,0,0,0, DateTimeKind.Utc), Base = Currency.USD, Quote= Currency.EUR, Value= 1.2111M},
            new Rate { Id=3, UnixTs = new DateTime(2025,1,3, 0,0,0,0, DateTimeKind.Utc), Base = Currency.USD, Quote= Currency.EUR, Value= 1.3111M},
            new Rate { Id=4, UnixTs = new DateTime(2025,1,4, 0,0,0,0, DateTimeKind.Utc), Base = Currency.USD, Quote= Currency.EUR, Value= 1.4111M},
        ]
        );
    }
}

public class Rate
{
    public int Id { get; set; }
    public DateTime UnixTs { get; set; }
    public Currency Base { get; set; }
    public Currency Quote { get; set; }
    public decimal Value { get; set; }
}
