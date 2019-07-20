using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Skelvy.WebAPI.Extensions
{
  public static class RedisExtension
  {
    public static void AddRedisDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddStackExchangeRedisCache(options =>
      {
        options.Configuration = configuration["SKELVY_REDIS_CONNECTION"];
      });

      var connection = ConnectionMultiplexer.Connect(configuration["SKELVY_REDIS_CONNECTION"]);
      services.AddSingleton<IConnectionMultiplexer>(connection);
    }
  }
}
