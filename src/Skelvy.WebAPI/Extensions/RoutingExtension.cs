using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.WebAPI.Filters;

namespace Skelvy.WebAPI.Extensions
{
  public static class RoutingExtension
  {
    public static void AddCustomRouting(this IServiceCollection services)
    {
      services.AddMvc(options =>
        {
          options.Filters.Add(typeof(CustomExceptionFilter));
          options.Filters.Add(new AuthorizeFilter(
            new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build()));
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
        .AddMvcOptions(options =>
        {
          options.ModelMetadataDetailsProviders.Clear();
          options.ModelValidatorProviders.Clear();
        });

      services.AddApiVersioning(o =>
      {
        o.DefaultApiVersion = new ApiVersion(2, 0);
        o.AssumeDefaultVersionWhenUnspecified = true;
      });

      services.AddVersionedApiExplorer();
    }
  }
}
