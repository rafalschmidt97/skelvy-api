using System.Reflection;
using Coravel;
using Coravel.Invocable;
using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skelvy.WebAPI.Infrastructure.Notifications;
using Skelvy.WebAPI.Schedulers;

namespace Skelvy.WebAPI
{
  public static class WebAPIStartup
  {
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses()
          .AsMatchingInterface());

      services.AddSignalR();
      services.AddSingleton<SignalRBackplane>();

      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses(classes => classes.AssignableTo(typeof(IInvocable)))
          .AsSelf()
          .WithTransientLifetime());
      services.AddScheduler();

      services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorRenderer>(sp =>
        new RazorRenderer(typeof(Program))));

      return services;
    }

    public static void UseWebAPI(this IApplicationBuilder app)
    {
      app.ApplicationServices.UseScheduler(scheduler =>
      {
        scheduler.Schedule<RemoveUsersScheduler>().Daily();
        scheduler.Schedule<RemoveExpiredMeetingsScheduler>().Hourly();
        scheduler.Schedule<RemoveExpiredMeetingRequestsScheduler>().Hourly();
      }).OnError(exception => throw exception);
    }
  }
}
