using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.Infrastructure
{
  public static class InfrastructureStartup
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses()
          .AsMatchingInterface()
          .WithTransientLifetime());

      return services;
    }
  }
}
