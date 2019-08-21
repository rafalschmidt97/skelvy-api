using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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

        var tokenName = "Access Token";

        configuration.AddSecurityDefinition(tokenName, new ApiKeyScheme
        {
          In = "header",
          Name = "Authorization",
          Type = "apiKey",
        });

        configuration.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
        {
          { tokenName, Array.Empty<string>() },
        });

        configuration.OperationFilter<SwaggerDefaultValues>();
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

    private static Info CreateInfoForApiVersion(ApiVersionDescription description)
    {
      var info = new Info
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

  public class SwaggerDefaultValues : IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      var apiDescription = context.ApiDescription;

      operation.Deprecated = apiDescription.IsDeprecated();

      if (operation.Parameters == null)
      {
        return;
      }

      foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
      {
        var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

        if (parameter.Description == null)
        {
          parameter.Description = description.ModelMetadata?.Description;
        }

        if (parameter.Default == null)
        {
          parameter.Default = description.DefaultValue;
        }

        parameter.Required |= description.IsRequired;
      }
    }
  }
}
