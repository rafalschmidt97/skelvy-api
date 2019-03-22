using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Skelvy.WebAPI.Extensions
{
  public static class SwaggerExtension
  {
    public static void AddCustomSwagger(this IServiceCollection services)
    {
      services.AddSwaggerGen(configuration =>
      {
        configuration.SwaggerDoc("v1", new Info
        {
          Title = "Skelvy API",
          Description = "Mobile app for meetings over beer or coffee 🚀"
        });

        configuration.AddSecurityDefinition("Token", new ApiKeyScheme
        {
          In = "header",
          Name = "Authorization",
          Type = "apiKey"
        });

        configuration.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
        {
          { "Token", Array.Empty<string>() }
        });
      });
    }

    public static void UseCustomSwagger(this IApplicationBuilder app)
    {
      app.UseSwagger(options =>
      {
        options.PreSerializeFilters.Add((document, request) =>
        {
          document.Paths =
            document.Paths.ToDictionary(path => path.Key.ToLowerInvariant(), p => p.Value);
        });
      });
      app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));
    }
  }
}
