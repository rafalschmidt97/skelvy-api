using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skelvy.WebAPI.Infrastructure.Notifications;

namespace Skelvy.WebAPI.Extensions
{
  public static class EmailExtension
  {
    public static void AddEmail(this IServiceCollection services)
    {
      services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorRenderer>(sp =>
        new RazorRenderer(typeof(EmailNotificationsService))));
    }
  }
}
