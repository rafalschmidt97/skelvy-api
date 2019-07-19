using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.WebAPI.Hubs;

namespace Skelvy.WebAPI.Extensions
{
  public static class SocketExtension
  {
    public static void AddSocket(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddSignalR().AddStackExchangeRedis(configuration["SKELVY_REDIS_CONNECTION"]);
    }

    public static void UseSocket(this IApplicationBuilder app)
    {
      app.UseSignalR(options =>
      {
        options.MapHub<UsersHub>("/users");
      });
    }
  }
}
