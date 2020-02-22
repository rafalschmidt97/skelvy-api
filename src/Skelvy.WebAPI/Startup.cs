using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skelvy.Application;
using Skelvy.Infrastructure;
using Skelvy.Persistence;
using Skelvy.WebAPI.Extensions;
using Skelvy.WebAPI.Hubs;
using Skelvy.WebAPI.Infrastructure.Notifications;

namespace Skelvy.WebAPI
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      _configuration = configuration;
      _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddWebAPI();
      services.AddInfrastructure();
      services.AddPersistence(_configuration);
      services.AddApplication();

      if (_environment.IsDevelopment())
      {
        services.AddCustomOpenApi();
      }

      services.AddHealthChecks().AddDbContextCheck<SkelvyContext>();

      services.AddCors();

      services.AddCustomRouting();
      services.AddAuth(_configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
      var versionProvider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
      var backplane = app.ApplicationServices.GetService<SignalRBackplane>();

      app.UseWebAPI();

      if (_environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        app.UseCustomOpenApi(versionProvider);
      }

      app.UseCustomCors();

      app.UseCustomRouting();
      app.UseAuth();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<UsersHub>("/users");
        endpoints.MapHealthChecks("/health");
        endpoints.MapControllers();
      });

      backplane.Start();
    }
  }
}
