using Microsoft.Extensions.DependencyInjection;
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
  }
}
