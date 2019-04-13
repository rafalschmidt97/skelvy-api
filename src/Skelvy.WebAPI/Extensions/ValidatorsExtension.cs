using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.WebAPI.Extensions
{
  public static class ValidatorsExtension
  {
    public static void AddValidators(this IServiceCollection services)
    {
      var applicationAssembly = typeof(IMapsService).GetTypeInfo().Assembly;

      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());
    }
  }
}
