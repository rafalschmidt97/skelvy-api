using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.WebAPI.Extensions
{
  public static class MediatrExtension
  {
    public static void AddMediatr(this IServiceCollection services)
    {
      var applicationAssembly = typeof(IMapsService).GetTypeInfo().Assembly;

      services.AddMediatR(applicationAssembly);
      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());
    }
  }
}
