using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Skelvy.WebAPI.Extensions
{
  public static class SwaggerExtension
  {
    public static void AddCustomSwagger(this IServiceCollection services)
    {
      services.AddSwaggerGen(configuration =>
      {
        var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
          configuration.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description =
            "Enter 'Bearer' [space] and then your token in the text input.",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
        });

        configuration.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer",
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          },
        });
      });
    }

    public static void UseCustomSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        foreach (var description in provider.ApiVersionDescriptions)
        {
          options.SwaggerEndpoint(
              $"/swagger/{description.GroupName}/swagger.json",
              description.GroupName.ToUpperInvariant());
        }
      });
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
      var info = new OpenApiInfo
      {
        Title = $"Skelvy API {description.ApiVersion}",
        Description = "Mobile app for meetings over your favorite activities 🚀",
        Version = description.ApiVersion.ToString(),
      };

      if (description.IsDeprecated)
      {
        info.Description += " This API version has been deprecated.";
      }

      return info;
    }
  }
}
