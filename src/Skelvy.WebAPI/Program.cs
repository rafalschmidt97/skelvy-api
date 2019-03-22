﻿using Destructurama;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Skelvy.Persistence;

namespace Skelvy.WebAPI
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateWebHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var context = scope.ServiceProvider.GetService<SkelvyContext>();
        context.Database.Migrate();

        var hosting = scope.ServiceProvider.GetService<IHostingEnvironment>();
        if (hosting.IsDevelopment())
        {
          SkelvyInitializer.Initialize(context);
        }
      }

      host.Run();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseKestrel()
        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
          .ReadFrom.Configuration(hostingContext.Configuration)
          .Destructure.UsingAttributes()
          .Enrich.FromLogContext())
        .UseStartup<Startup>();
  }
}