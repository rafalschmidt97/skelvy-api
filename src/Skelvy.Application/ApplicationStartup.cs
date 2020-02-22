using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.Application
{
  public static class ApplicationStartup
  {
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
      services.AddMediatR(Assembly.GetExecutingAssembly());
      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());

      services.AddAutoMapper(Assembly.GetExecutingAssembly());

      services.Scan(scan =>
        scan.FromAssemblies(Assembly.GetExecutingAssembly())
          .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());

      return services;
    }
  }
}
