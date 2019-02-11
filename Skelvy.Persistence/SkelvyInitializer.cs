using System;
using System.Globalization;
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
      SeedProfiles(context);
    }

    private static void SeedUsers(SkelvyContext context)
    {
      if (context.Users.Any())
      {
        return;
      }

      var users = new[]
      {
        new User { Email = "user@gmail.com", FacebookId = "1" }
      };

      context.Users.AddRange(users);
      context.SaveChanges();
    }

    private static void SeedProfiles(SkelvyContext context)
    {
      if (context.UserProfiles.Any())
      {
        return;
      }

      var users = context.Users.ToList();

      var profiles = new[]
      {
        new UserProfile
        {
          Name = "User",
          Birthday = DateTime.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture),
          Gender = "male", // TODO: Fix circular dependency with GenderTypes from Application layer
          UserId = users[0].Id
        }
      };

      context.UserProfiles.AddRange(profiles);
      context.SaveChanges();

      var photos = new[]
      {
        new UserProfilePhoto
        {
          Url = "https://via.placeholder.com/1000/ebebf0/ffffff?text=1",
          ProfileId = profiles[0].Id
        }
      };

      context.UserProfilePhotos.AddRange(photos);
      context.SaveChanges();
    }
  }
}
