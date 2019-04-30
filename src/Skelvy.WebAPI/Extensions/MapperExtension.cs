using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.WebAPI.Extensions
{
  public static class MapperExtension
  {
    public static void AddMapper(this IServiceCollection services)
    {
      var applicationAssembly = typeof(IMapsService).GetTypeInfo().Assembly;
      services.AddAutoMapper(applicationAssembly);
    }
  }
}
