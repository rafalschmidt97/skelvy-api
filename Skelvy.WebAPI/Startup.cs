using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Core.Pipes;
using Skelvy.Persistence;
using Skelvy.WebAPI.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Skelvy.WebAPI
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      var applicationAssembly = typeof(RequestLogger<>).GetTypeInfo().Assembly;

      services.AddDbContext<SkelvyContext>(options =>
        options.UseSqlServer(_configuration.GetConnectionString("Database")));

      services.AddSwaggerGen(configuration =>
      {
        configuration.SwaggerDoc("v1", new Info
        {
          Title = "Skelvy API",
          Description = "Mobile app for meetings over beer or coffee 🚀"
        });
      });

      // Add Mediatr
      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
      services.AddMediatR(applicationAssembly);

      // Add AutoMapper
      services.AddAutoMapper(applicationAssembly);

      // Add Validators
      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());
      services.AddMvc(options =>
        {
          options.Filters.Add(typeof(CustomExceptionFilter));
          options.AllowValidatingTopLevelNodes = false;
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseSwagger();
      app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));

      app.UseMvc();
    }
  }
}
