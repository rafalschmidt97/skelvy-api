using System.Reflection;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Core.Pipes;
using Skelvy.Infrastructure.Notifications;
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
      var applicationAssembly = typeof(RequestLogger<,>).GetTypeInfo().Assembly;
      var infrastructureAssembly = typeof(NotificationService).GetTypeInfo().Assembly;

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

      // Add Services
      services.Scan(scan =>
        scan.FromAssemblies(infrastructureAssembly)
          .AddClasses()
          .AsMatchingInterface());

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
          };
        });

      services.AddMvc(options =>
        {
          options.Filters.Add(typeof(CustomExceptionFilter));
          options.AllowValidatingTopLevelNodes = false;
          options.Filters.Add(new AuthorizeFilter(
            new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build()));
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseSwagger();
      app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));

      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
