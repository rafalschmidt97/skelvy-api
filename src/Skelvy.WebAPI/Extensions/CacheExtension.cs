using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.WebAPI.Extensions
{
  public static class CacheExtension
  {
    public static void AddCacheDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDistributedRedisCache(options =>
      {
        options.Configuration = configuration.GetConnectionString("Redis");
      });
    }
  }
}
