using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.WebAPI.Filters;

namespace Skelvy.WebAPI.Extensions
{
  public static class MvcExtension
  {
    public static void AddCustomMvc(this IServiceCollection services)
    {
      services.AddMvc(options =>
        {
          options.Filters.Add(typeof(CustomExceptionFilter));
          options.Filters.Add(new AuthorizeFilter(
            new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build()));
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .AddMvcOptions(options =>
        {
          options.ModelMetadataDetailsProviders.Clear();
          options.ModelValidatorProviders.Clear();
        });

      services.AddApiVersioning(o =>
      {
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.AssumeDefaultVersionWhenUnspecified = true;
      });

      services.AddVersionedApiExplorer();
    }

    public static void UseCustomMvc(this IApplicationBuilder app)
    {
      app.UseStaticFiles();
      app.UseMvc();
    }
  }
}
