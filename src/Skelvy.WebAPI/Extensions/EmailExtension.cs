using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Skelvy.WebAPI.Extensions
{
  public static class EmailExtension
  {
    public static void AddEmail(this IServiceCollection services)
    {
      services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorRenderer>());
    }
  }
}
