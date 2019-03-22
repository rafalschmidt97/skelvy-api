using Microsoft.AspNetCore.Builder;

namespace Skelvy.WebAPI.Extensions
{
  public static class CorsExtension
  {
    public static void UseCustomCors(this IApplicationBuilder app)
    {
      app.UseCors(builder => builder
        .SetIsOriginAllowed(isOriginAllowed => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
    }
  }
}
