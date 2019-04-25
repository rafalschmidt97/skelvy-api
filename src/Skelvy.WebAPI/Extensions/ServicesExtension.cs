using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Infrastructure.Maps.GoogleMaps;
using Skelvy.Persistence.Repositories;

namespace Skelvy.WebAPI.Extensions
{
  public static class ServicesExtension
  {
    public static void AddServices(this IServiceCollection services)
    {
      var infrastructureAssembly = typeof(MapsService).GetTypeInfo().Assembly;
      var presentationAssembly = typeof(Startup).GetTypeInfo().Assembly;
      var persistenceAssembly = typeof(UsersRepository).GetTypeInfo().Assembly;

      services.Scan(scan =>
        scan.FromAssemblies(infrastructureAssembly, presentationAssembly, persistenceAssembly)
          .AddClasses()
          .AsMatchingInterface());
    }
  }
}
