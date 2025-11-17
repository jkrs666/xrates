using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Moq;

[CollectionDefinition("Sequential", DisableParallelization = true)]
public class SequentialCollection { }

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
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var dbFactory = new PooledDbContextFactory<AppDbContext>(options);
        var logger = Mock.Of<ILogger<ExternalApiService>>();
        var redis = RedisConnectionFactory.Create();
        var convertService = new ConvertService(logger);
        return new RepositoryService(logger, dbFactory, redis, convertService);
    }

}
