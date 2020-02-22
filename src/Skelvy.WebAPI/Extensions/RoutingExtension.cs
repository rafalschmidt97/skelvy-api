using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.WebAPI.Extensions
{
  public static class RoutingExtension
  {
    public static void AddCustomRouting(this IServiceCollection services)
    {
      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses(classes => classes.AssignableTo(typeof(IMiddleware)))
          .AsSelf()
          .WithTransientLifetime());

      services
        .AddControllers()
        .AddNewtonsoftJson()
        .AddMvcOptions(options =>
      {
        options.ModelMetadataDetailsProviders.Clear();
        options.ModelValidatorProviders.Clear();
      });

      services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(2, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
      });

      services.AddVersionedApiExplorer();
    }

    public static void UseCustomRouting(this IApplicationBuilder app)
    {
      app.UseCustomExceptionHandler();
      app.UseRouting();
    }
  }
}
