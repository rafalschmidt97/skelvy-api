using System.Linq;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence
{
  public static class SkelvyInitializer
  {
    public static void Initialize(SkelvyContext context)
    {
      SeedEverything(context);
    }

    private static void SeedEverything(SkelvyContext context)
    {
      context.Database.EnsureCreated();
      SeedUsers(context);
    }

    private static void SeedUsers(SkelvyContext context)
    {
      if (context.Users.Any())
      {
        return;
      }

      var users = new[]
      {
        new User { Email = "user1@gmail.com" },
        new User { Email = "user2@gmail.com" },
        new User { Email = "admin@gmail.com" }
      };

      context.Users.AddRange(users);
      context.SaveChanges();
    }
  }
}
