using System.Reflection;
using Coravel;
using Coravel.Invocable;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.WebAPI.Schedulers;

namespace Skelvy.WebAPI.Extensions
{
  public static class SchedulersExtension
  {
    public static void AddSchedulers(this IServiceCollection services)
    {
      var presentationAssembly = typeof(Startup).GetTypeInfo().Assembly;

      services.Scan(scan =>
        scan.FromAssemblies(presentationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IInvocable)))
          .AsSelf()
          .WithTransientLifetime());
      services.AddScheduler();
    }

    public static void UseSchedulers(this IApplicationBuilder app)
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
