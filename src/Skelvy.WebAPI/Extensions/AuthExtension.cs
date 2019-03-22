using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Skelvy.WebAPI.Extensions
{
  public static class AuthExtension
  {
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
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
    }

    public static void UseAuth(this IApplicationBuilder app)
    {
      app.UseAuthentication();
    }
  }
}
