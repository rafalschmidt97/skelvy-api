using System.Reflection;
using AutoMapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.WebAPI.Extensions
{
  public static class MediatrExtension
  {
    public static void AddMediatr(this IServiceCollection services)
    {
      var applicationAssembly = typeof(IMapsService).GetTypeInfo().Assembly;

      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
      services.AddMediatR(applicationAssembly);

      services.AddAutoMapper(applicationAssembly);
    }
  }
}
