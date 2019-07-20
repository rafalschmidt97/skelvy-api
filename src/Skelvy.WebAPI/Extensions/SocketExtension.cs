using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.WebAPI.Hubs;
using Skelvy.WebAPI.Infrastructure.Notifications;

namespace Skelvy.WebAPI.Extensions
{
  public static class SocketExtension
  {
    public static void AddSocket(this IServiceCollection services)
    {
      services.AddSignalR();
      services.AddSingleton<SignalRBackplane>();
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
