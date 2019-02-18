using System;
using System.Globalization;
using System.Linq;
using Skelvy.Common;
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
      SeedDrinks(context);
      SeedMeetingRequests(context);
    }

    private static void SeedUsers(SkelvyContext context)
    {
      if (context.Users.Any())
      {
        return;
      }

      var users = new[]
      {
        new User { Email = "user@gmail.com", FacebookId = "1", GoogleId = "1" }
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
          Gender = GenderTypes.Male,
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

    private static void SeedDrinks(SkelvyContext context)
    {
      if (context.Drinks.Any())
      {
        return;
      }

      var drinks = new[]
      {
        new Drink { Name = "tea" },
        new Drink { Name = "chocolate" },
        new Drink { Name = "coffee" },
        new Drink { Name = "beer" },
        new Drink { Name = "wine" },
        new Drink { Name = "vodka" },
        new Drink { Name = "whiskey" }
      };

      context.Drinks.AddRange(drinks);
      context.SaveChanges();
    }

    private static void SeedMeetingRequests(SkelvyContext context)
    {
      if (context.UserProfiles.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var drinks = context.Drinks.ToList();

      var requests = new[]
      {
        new MeetingRequest
        {
          MinDate = DateTime.Now,
          MaxDate = DateTime.Now.AddDays(2),
          MinAge = 18,
          MaxAge = 25,
          Latitude = 1,
          Longitude = 1,
          UserId = users[0].Id
        }
      };

      context.MeetingRequests.AddRange(requests);
      context.SaveChanges();

      var requestsDrinks = new[]
      {
        new MeetingRequestDrink
        {
          MeetingRequestId = requests[0].Id,
          DrinkId = drinks[0].Id
        }
      };

      context.MeetingRequestDrinks.AddRange(requestsDrinks);
      context.SaveChanges();
    }
  }
}
