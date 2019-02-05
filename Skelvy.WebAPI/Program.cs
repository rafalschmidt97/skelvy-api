using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Persistence;

namespace Skelvy.WebAPI
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateWebHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var context = scope.ServiceProvider.GetService<SkelvyContext>();
        context.Database.Migrate();
        SkelvyInitializer.Initialize(context);
      }

      host.Run();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
  }
}
