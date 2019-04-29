using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Persistence;
using Skelvy.WebAPI.Extensions;

namespace Skelvy.WebAPI
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateWebHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var hosting = scope.ServiceProvider.GetService<IHostingEnvironment>();
        if (hosting.IsDevelopment())
        {
          var context = scope.ServiceProvider.GetService<SkelvyContext>();
          context.Database.Migrate();
          context.Database.EnsureCreated();
          SkelvyInitializer.Initialize(context);
        }
      }

      host.Run();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseKestrel()
        .UseLogger()
        .UseStartup<Startup>();
  }
}
