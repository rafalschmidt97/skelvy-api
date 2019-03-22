using Destructurama;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Skelvy.WebAPI.Extensions
{
  public static class LoggerExtension
  {
    public static IWebHostBuilder UseLogger(this IWebHostBuilder builder)
    {
      return builder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Destructure.UsingAttributes()
        .Enrich.FromLogContext());
    }
  }
}
