using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.WebAPI.Extensions;

namespace Skelvy.WebAPI
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSqlDatabase(_configuration);
      services.AddCacheDatabase(_configuration);
      services.AddCustomSwagger();
      services.AddHealthChecks();
      services.AddMediatr();
      services.AddMapper();
      services.AddValidators();
      services.AddServices();
      services.AddSchedulers();
      services.AddCors();
      services.AddAuth(_configuration);
      services.AddSocket();
      services.AddEmail();
      services.AddCustomMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
    {
      if (env.IsDevelopment())
      {
        app.UseCustomSwagger(provider);
      }

      app.UseHealthChecks("/");
      app.UseSchedulers();
      app.UseCustomCors();
      app.UseAuth();
      app.UseSocket();
      app.UseCustomMvc();
    }
  }
}
