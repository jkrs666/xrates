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

builder.Services.AddScoped<ConvertService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddHttpClient<ExternalApiService>();
builder.Services.AddHostedService<InitializationService>();
builder.Services.AddHostedService<PeriodicFetchService>();
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new DecimalConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHttpLogging();

app.Run();
