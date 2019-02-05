using System.Reflection;
using AutoMapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Core.Pipes;

namespace Skelvy.WebAPI
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      var applicationAssembly = typeof(RequestLogger<>).GetTypeInfo().Assembly;

      // Add Mediatr
      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
      services.AddMediatR(applicationAssembly);

      // Add AutoMapper
      services.AddAutoMapper(applicationAssembly);

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}
