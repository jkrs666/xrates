using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Moq;


public class RedisConnectionFactory
{
    public static IConnectionMultiplexer Create()
    {
        var configuration = ConfigurationOptions.Parse("localhost:7777,allowAdmin=true");
        configuration.AbortOnConnectFail = false;
        return ConnectionMultiplexer.Connect(configuration);
    }

    public static void ClearCache()
    {
        var redis = RedisConnectionFactory.Create();
        var endpoint = redis.GetEndPoints().First();
        redis.GetServer(endpoint).FlushAllDatabases();
    }
}

public class RepositoryServiceFactory
{
    public static RepositoryService Create()
    {
        var dbOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql("Host=localhost;Port=9999;Database=postgres;Username=postgres;Password=pg_test")
        .UseLowerCaseNamingConvention()
        .Options;
        var context = new AppDbContext(dbOptionsBuilder);
        context.Database.EnsureCreated();

        var logger = Mock.Of<ILogger<RepositoryService>>();
        var logger2 = Mock.Of<ILogger<ConvertService>>();

        var redis = RedisConnectionFactory.Create();
        var convertService = new ConvertService(logger2);
        return new RepositoryService(logger, context, redis, convertService);
    }

}
