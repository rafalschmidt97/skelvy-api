using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Coravel;
using Coravel.Invocable;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Skelvy.Application.Core.Pipes;
using Skelvy.Infrastructure;
using Skelvy.Persistence;
using Skelvy.WebAPI.Filters;
using Skelvy.WebAPI.Hubs;
using Skelvy.WebAPI.Schedulers;
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
      var infrastructureAssembly = typeof(HttpServiceBase).GetTypeInfo().Assembly;
      var presentationAssembly = typeof(Startup).GetTypeInfo().Assembly;

      services.AddDbContext<SkelvyContext>(options =>
        options.UseSqlServer(_configuration.GetConnectionString("Database")));

      services.AddDistributedRedisCache(options =>
      {
        options.Configuration = _configuration.GetConnectionString("Redis");
      });

      services.AddSwaggerGen(configuration =>
      {
        configuration.SwaggerDoc("v1", new Info
        {
          Title = "Skelvy API",
          Description = "Mobile app for meetings over beer or coffee 🚀"
        });

        configuration.AddSecurityDefinition("Token", new ApiKeyScheme
        {
          In = "header",
          Name = "Authorization",
          Type = "apiKey"
        });

        configuration.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
        {
          { "Token", Array.Empty<string>() }
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

      services.AddAutoMapper(applicationAssembly);

      // Add Validators
      services.Scan(scan =>
        scan.FromAssemblies(applicationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
          .AsImplementedInterfaces()
          .WithTransientLifetime());

      // Add Services
      services.Scan(scan =>
        scan.FromAssemblies(infrastructureAssembly, presentationAssembly)
          .AddClasses()
          .AsMatchingInterface());

      // Add Schedulers
      services.Scan(scan =>
        scan.FromAssemblies(presentationAssembly)
          .AddClasses(classes => classes.AssignableTo(typeof(IInvocable)))
          .AsSelf()
          .WithTransientLifetime());
      services.AddScheduler();

      services.AddCors();

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

          options.Events = new JwtBearerEvents
          {
            OnMessageReceived = context =>
            {
              if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
              {
                var token = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token))
                {
                  context.Token = token;
                }
              }

              return Task.CompletedTask;
            }
          };
        });

      services.AddSignalR();

      services.AddMvc(options =>
        {
          options.Filters.Add(typeof(CustomExceptionFilter));
          options.Filters.Add(new AuthorizeFilter(
            new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build()));
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .AddMvcOptions(options =>
        {
          options.ModelMetadataDetailsProviders.Clear();
          options.ModelValidatorProviders.Clear();
        });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseSwagger(options =>
        {
          options.PreSerializeFilters.Add((document, request) =>
          {
            document.Paths =
              document.Paths.ToDictionary(path => path.Key.ToLowerInvariant(), p => p.Value);
          });
        });
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));
      }

      app.ApplicationServices.UseScheduler(scheduler =>
      {
        scheduler.Schedule<RemoveExpiredMeetingsScheduler>().Daily();
        scheduler.Schedule<RemoveExpiredMeetingRequestsScheduler>().Daily();
        scheduler.Schedule<MatchMeetingRequestsScheduler>().Hourly();
      }).OnError(exception => throw exception);

      app.UseCors(builder => builder
        .SetIsOriginAllowed(isOriginAllowed => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());

      app.UseAuthentication();

      app.UseSignalR(options =>
      {
        options.MapHub<UsersHub>("/api/users");
      });

      app.UseStaticFiles();

      app.UseMvc();
    }
  }
}
