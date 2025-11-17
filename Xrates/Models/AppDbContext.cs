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
        modelBuilder.Entity<Rate>()
        .HasData(
            [
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,1,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.1011M)      {Id=1},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,1,  2,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.1211M)      {Id=2},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,1,  1,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.1111M)      {Id=3},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,2,  3,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.2311M)      {Id=4},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,2,  2,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.2211M)      {Id=5},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,2,  4,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.2411M)      {Id=6},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,2,  1,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.2111M)      {Id=7},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,3,  1,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.3111M)      {Id=8},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,3,  2,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.3211M)      {Id=9},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,3,  3,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.3311M)      {Id=10},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,4,  3,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.4311M)      {Id=11},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,4,  2,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.4211M)      {Id=12},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,4,  1,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  1.4111M)      {Id=13},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,5,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "EUR",  Value:  0.86057873M)  {Id=14},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,1,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "CHF",  Value:  2.1000M)      {Id=15},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,2,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "CHF",  Value:  2.2000M)      {Id=16},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,3,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "CHF",  Value:  2.3000M)      {Id=17},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,4,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "CHF",  Value:  2.4000M)      {Id=18},
        new  Rate  (  Timestamp  :  new  DateTime(2025,1,5,  0,0,0,0,  DateTimeKind.Utc),  Base: "USD", Quote:  "CHF",  Value:  0.79409917M)  {Id=19},
        ]
        );

        modelBuilder.Entity<Integration>().HasKey(r => r.Name);
        modelBuilder.Entity<Integration>()
        .HasData(
            [
            new Integration ( Name:"errorTest", Url:"invalidUrl", FreqSeconds: 10, Priority:0, Enabled:true),
            new Integration ( Name:"example", Url:"https://example.com", FreqSeconds: 10, Priority:0, Enabled:true),
            new Integration ( Name:"frankfurter", Url:"https://api.frankfurter.dev/v1/latest?base=USD", FreqSeconds: 10, Priority:1, Enabled:true)
        ]
        );
    }
}

