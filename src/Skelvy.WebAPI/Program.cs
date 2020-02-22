using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skelvy.Persistence;
using Skelvy.WebAPI.Extensions;

namespace Skelvy.WebAPI
{
  public static class Program
  {
    public static readonly string InstanceId = Guid.NewGuid().ToString();

    public static void Main(string[] args)
    {
      Console.WriteLine($"Instance Id: {InstanceId}");
      var host = CreateWebHostBuilder(args).Build();

      using var scope = host.Services.CreateScope();
      var hosting = scope.ServiceProvider.GetService<IWebHostEnvironment>();
      if (hosting.IsDevelopment())
      {
        var context = scope.ServiceProvider.GetService<SkelvyContext>();
        context.Database.Migrate();
        context.Database.EnsureCreated();
        SkelvyInitializer.Initialize(context);
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
