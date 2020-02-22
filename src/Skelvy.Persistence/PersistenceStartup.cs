using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Skelvy.Persistence
{
  public static class PersistenceStartup
  {
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddStackExchangeRedisCache(options =>
      {
        options.Configuration = configuration["SKELVY_REDIS_CONNECTION"];
      });

      var connection = ConnectionMultiplexer.Connect(configuration["SKELVY_REDIS_CONNECTION"]);
      services.AddSingleton<IConnectionMultiplexer>(connection);

      services.AddDbContext<SkelvyContext, SkelvyContext>(options =>
        options.UseSqlServer(configuration["SKELVY_SQL_CONNECTION"]));

      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses()
          .AsMatchingInterface()
          .WithTransientLifetime());

      return services;
    }
  }
}
