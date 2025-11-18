using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }


    public DbSet<Rate> Rates { get; set; }
    public DbSet<Integration> Integrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rate>().HasKey(r => r.Id);
        modelBuilder.Entity<Rate>().Property(r => r.Id).UseIdentityAlwaysColumn();

        modelBuilder.Entity<Integration>().HasKey(r => r.Name);
        modelBuilder.Entity<Integration>()
        .HasData(
            [
            new Integration ( Name:"errorTest", Url:"invalidUrl", FreqSeconds: 10, Priority:0, Enabled:true, "USD", "date", "rates"),
            new Integration ( Name:"example", Url:"https://example.com", FreqSeconds: 10, Priority:0, Enabled:true, "USD", "timestamp", "rates"),
            new Integration ( Name:"frankfurter", Url:"https://api.frankfurter.dev/v1/latest?base=USD", FreqSeconds: 10, Priority:1, Enabled:true, "USD", "date", "rates")
        ]
        );
    }
}

