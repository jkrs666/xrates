# how to run

```bash
git clone https://github.com/jkrs666/xrates.git
cd xrates
./recreate.sh
```

`./recreate.sh` drops all docker volumes and rebuilds the app

visit [http://localhost](http://localhost) for endpoint information and to run test requests (swagger UI)

## run unit and integration tests
```bash
./test.sh
```

## generate test coverage report
```bash
./gen_reports.sh
```

this generates and opens `./coveragereport/index.html` with firefox

## db migrations

migrations are automatically applied on startup with `InitializationService`

you can also execute `./run_migrations.sh` to run them manually


# Documentation

## Schema

### Rate

```C#
public record Rate
(
    DateTime Timestamp,
    String Base,
    String Quote,
    decimal Value
)
{
    public int Id { get; set; }
};
```

the DTO for Rate

value is the actual rate, Base/Quote

`decimal` type was chosen because of its higher precision digits

`Id` is mutable so the `DbContext` will auto-increment

### RateCompact
```C#
public record RateCompact
(
    DateTime Ts,
    decimal Rate
);

```
this is a minimal structure that is used in response objects

### Integration
```C#
public record Integration
(
    string Name,
    string Url,
    int FreqSeconds,
    int Priority,
    bool Enabled,
    string BaseCurrency,
    string timestampJsonField,
    string ratesJsonField
//TODO: encrypt
//public string ApiKey { get; set; }
);
```

the DTO for Integrations (external rate APIs)

`Name` is the primary key (`id`) and must be set manually by the user

`Url` is the GET endpoint

`FreqSeconds` how often the request is repeated (in seconds)

`Priority` the smaller the number the higher the priority

`Enabled` if false the Integration will never run

`BaseCurrency` the base currency of the response. we need this to calculate the cartesian of rates

`timestampJsonField` the name of the response json field (from root) where the timestamp is

for example, it should be equal to `date` if the response has the following structure:
```json
{
    "base" : "USD",
    "date" : "2026-1-1",
    "rates" : {
        "EUR" : 0.123,
        "JPY" : 0.456
    }
}
```

`ratesJsonField` similarly to the above, it should be equal to `rates`

#### Missing implementations

API key encryption was not implemented

the current implementation is limited because it expects a certain structure

for more advanced response parsing and minimal code, **jq** [(jqlang.org)](https://jqlang.org/) could be used to transform the response to a preferred format. 

### AppDbContext

```C#
        modelBuilder.Entity<Rate>().HasKey(r => r.Id);
        modelBuilder.Entity<Rate>().HasIndex(r => new { r.Base, r.Quote });
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

```

sets primary keys and indexes

inits integrations

## Endpoints

```
Convert
GET /api/Convert

Historical
GET /api/Historical

Integrations
GET /api/Integrations
POST /api/Integrations
GET /api/Integrations/{id}
PUT /api/Integrations/{id}
DELETE /api/Integrations/{id}

Rates
GET /api/Rates
GET /api/Rates/{pair}
```

visit [http://localhost](http://localhost) for endpoint information and to run test requests (swagger UI)


## Services

### Convert Service

```C#
    public decimal CalculateConversionRate(decimal from, decimal to)
    {
        return (1 / from) * to;
    }
```

used to be bigger and convert currencies on the fly, but its not neccessary, since we are pre-calculating and storing anyway

has only one function which is used during cartesian rate combinations

basically if we want to calculate `EUR/CNY` but we only have `USD/EUR` and `USD/CNY`, then using this function

`CalculateConversionRate(from:USD/EUR, to:USD/CNY)` will produce `EUR/CNY`


### ExternalApiService

calls an integration as described above

### RepositoryService

handles all DB and cache queries 

```C#
public interface IRepositoryService
{
    Task<Dictionary<string, RateCompact>> GetAllRates();
    Task<RateCompact> GetRate(string pair);
    Task<List<Rate>> GetHistoricalRates(DateTime start, DateTime end, string @base, string quote);
    Task<IEnumerable<Integration>> GetIntegrations();
    Task<Integration> GetIntegrationById(string id);
    Task<int> CreateIntegration(Integration integration);
    Task<int> UpdateIntegration(string id, UpdateIntegrationParams uip);
    Task<int> DeleteIntegration(string id);
    Task<List<Integration>> GetEnabledIntegrationsSorted();
    Task<int> DisableIntegration(string id);
    Task<int> EnableIntegration(string id);
    int SaveRatesCombinations(DateTime timestamp, List<Rate> ratesUsd);
    Task RefreshCache();
}
```


### InitializationService

runs only once at startup

runs migrations

loads historical data from `./Backend/src/Xrates/data.json`

calculates cartesian combinations for all rates for all historical timestamps

saves them in DB

refreshes cache with the latest rates


```C#
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initialization started");
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
        var redis = scope.ServiceProvider.GetRequiredService<IDatabase>();
        var externalApiService = scope.ServiceProvider.GetRequiredService<ExternalApiService>();
        var repositoryService = scope.ServiceProvider.GetRequiredService<IRepositoryService>();

        _logger.LogInformation("Running migrations");
        await dbContext.Database.MigrateAsync();

        _logger.LogInformation("Load historical data");
        string jsonString = File.ReadAllText("./data.json");
        var data = JsonSerializer.Deserialize<HistoricalDataJson>(jsonString);

        foreach (var kv in data.Rates.ToList())
        {
            var rates = new List<Rate>();
            var date = DateTime.Parse(kv.Key).ToUniversalTime();
            kv.Value.ToList().ForEach(rkv => rates.Add(new Rate(date, "USD", rkv.Key, rkv.Value)));
            var inserted = repositoryService.SaveRatesCombinations(rates.Last().Timestamp, rates);
            _logger.LogInformation($"Inserted {inserted} rows");
        }

        _logger.LogInformation("Warming cache");
        await repositoryService.RefreshCache();

        _logger.LogInformation("Initialization finished");
    }
```

### PeriodicFetchService

runs constantly on background and fetches updates from the primary integration

if the primary integration fails it gets disabled and gets replaced by backup integrations based on priority field

```C#
    private async Task DoWork()
    {
        int count = Interlocked.Increment(ref _executionCount);
        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);

        var repositoryService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IRepositoryService>();
        var integrations = await repositoryService.GetEnabledIntegrationsSorted();
        var primaryIntegration = integrations.First();

        // tick, continue
        if (count != 0 && count % primaryIntegration.FreqSeconds != 0) return;

        bool success;
        success = await FetchAndInsert(primaryIntegration, repositoryService);
        if (success) return;

        var backupIntegrations = integrations.Skip(1);
        foreach (Integration i in backupIntegrations)
        {
            success = await FetchAndInsert(i, repositoryService);
            if (success) return;
        }

    }
```

for example, if we have 3 services 

- a {FreqSeconds=10, Priority=1}
- b {FreqSeconds=20, Priority=2}
- c {FreqSeconds=30, Priority=3}

count is increased by 1 each second

1. count = 0, call a -> ok
2. count = 1
3. count = 2
5. count = 10, call a -> fail, disable a, call b -> ok 
6. count = 20, call b -> ok 
7. count = 40, call b -> ok 
8. count = 60, call b -> fail, call c -> ok 
9. count = 90, call c -> ok 
10. count = 120, call c -> ok 
11. ...

#### Missing implementations

**re-enable services**

currently when a service gets disabled is not re-enabled

it must be done manually

a possible solution to this could be storing the failed integrations to Redis with expiration

when the TTL of the Redis value gets to 0 the integration would be returned by the `GetEnabledIntegrationsSorted` function

**monitoring and alerts**

this could be implemented with Prometheus/Graphana

the HTTP client would send metrics to Prometheus and Graphana could display dashboards grouped by HTTP status codes

alerts and thresholds can also be configured in Grafana
