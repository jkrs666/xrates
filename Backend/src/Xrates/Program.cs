using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
    o.LoggingFields = HttpLoggingFields.All;
});

Action<DbContextOptionsBuilder> dbOptionsBuilder = options =>
    options
    .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
    .UseLowerCaseNamingConvention();

builder.Services.AddDbContextFactory<AppDbContext>(dbOptionsBuilder);
builder.Services.AddDbContext<AppDbContext>();


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
            builder.Configuration.GetConnectionString("Redis") ?? ""
    );
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped<IDatabase>(sp =>
{
    var redis = sp.GetRequiredService<IConnectionMultiplexer>();
    return redis.GetDatabase();
});

builder.Services.AddScoped<RepositoryService>();
builder.Services.AddScoped<ConvertService>();
builder.Services.AddHttpClient<ExternalApiService>();
builder.Services.AddHostedService<InitializationService>();
builder.Services.AddHostedService<PeriodicFetchService>();
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new DecimalConverter()));
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHttpLogging();

app.Run();
