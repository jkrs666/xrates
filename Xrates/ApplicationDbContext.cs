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
        new  Rate  (  Id:1,   Timestamp  :  new  DateTime(2025,1,1,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.1011M),
        new  Rate  (  Id:2,   Timestamp  :  new  DateTime(2025,1,1,  2,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.1211M),
        new  Rate  (  Id:3,   Timestamp  :  new  DateTime(2025,1,1,  1,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.1111M),

        new  Rate  (  Id:4,   Timestamp  :  new  DateTime(2025,1,2,  3,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.2311M),
        new  Rate  (  Id:5,   Timestamp  :  new  DateTime(2025,1,2,  2,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.2211M),
        new  Rate  (  Id:6,   Timestamp  :  new  DateTime(2025,1,2,  4,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.2411M),
        new  Rate  (  Id:7,   Timestamp  :  new  DateTime(2025,1,2,  1,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.2111M),

        new  Rate  (  Id:8,   Timestamp  :  new  DateTime(2025,1,3,  1,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.3111M),
        new  Rate  (  Id:9,   Timestamp  :  new  DateTime(2025,1,3,  2,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.3211M),
        new  Rate  (  Id:10,   Timestamp  :  new  DateTime(2025,1,3,  3,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.3311M),

        new  Rate  (  Id:11,   Timestamp  :  new  DateTime(2025,1,4,  3,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.4311M),
        new  Rate  (  Id:12,  Timestamp  :  new  DateTime(2025,1,4,  2,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.4211M),
        new  Rate  (  Id:13,  Timestamp  :  new  DateTime(2025,1,4,  1,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  1.4111M),

        new  Rate  (  Id:14,  Timestamp  :  new  DateTime(2025,1,5,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "EUR",  Value:  0.86057873M),

        new  Rate  (  Id:15,  Timestamp  :  new  DateTime(2025,1,1,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "CHF",  Value:  2.1000M),
        new  Rate  (  Id:16,  Timestamp  :  new  DateTime(2025,1,2,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "CHF",  Value:  2.2000M),
        new  Rate  (  Id:17,  Timestamp  :  new  DateTime(2025,1,3,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "CHF",  Value:  2.3000M),
        new  Rate  (  Id:18,  Timestamp  :  new  DateTime(2025,1,4,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "CHF",  Value:  2.4000M),
        new  Rate  (  Id:19,  Timestamp  :  new  DateTime(2025,1,5,  0,0,0,0,  DateTimeKind.Utc),  Currency:  "CHF",  Value:  0.79409917M),
        ]
        );

        modelBuilder.Entity<Integration>().HasKey(r => r.Name);
        modelBuilder.Entity<Integration>()
        .HasData(
            [
            new Integration ( Name:"frankfurter", Url:"https://api.frankfurter.dev/v1/latest?base=USD")
        ]
        );
    }
}

