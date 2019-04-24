using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skelvy.Application.Core.Persistence;
using Skelvy.Persistence;

namespace Skelvy.WebAPI.Extensions
{
  public static class DatabaseExtension
  {
    public static void AddSqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ISkelvyContext, SkelvyContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Database")));
    }
  }
}
