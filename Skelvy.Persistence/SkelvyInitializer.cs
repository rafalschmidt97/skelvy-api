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
      context.Database.EnsureCreated();
      SeedUsers(context);
      SeedProfiles(context);
      SeedDrinks(context);
      SeedMeetingRequests(context);
      SeedMeetings(context);
    }

    public static void SeedUsers(SkelvyContext context)
    {
      if (context.Users.Any())
      {
        return;
      }

      var users = new[]
      {
        new User { Email = "user1@gmail.com", FacebookId = "1", GoogleId = "1" },
        new User { Email = "user2@gmail.com", FacebookId = "2", GoogleId = "2" },
        new User { Email = "user3@gmail.com", FacebookId = "3", GoogleId = "3" }
      };

      context.Users.AddRange(users);
      context.SaveChanges();
    }

    public static void SeedProfiles(SkelvyContext context)
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
          Name = "User1",
          Birthday = DateTime.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture).Date,
          Gender = GenderTypes.Male,
          UserId = users[0].Id
        },
        new UserProfile
        {
          Name = "User2",
          Birthday = DateTime.ParseExact("22/04/1996", "dd/MM/yyyy", CultureInfo.CurrentCulture).Date,
          Gender = GenderTypes.Male,
          UserId = users[1].Id
        },
        new UserProfile
        {
          Name = "User3",
          Birthday = DateTime.ParseExact("22/04/1995", "dd/MM/yyyy", CultureInfo.CurrentCulture).Date,
          Gender = GenderTypes.Male,
          UserId = users[2].Id
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
        },
        new UserProfilePhoto
        {
          Url = "https://via.placeholder.com/1000/ebebf0/ffffff?text=2",
          ProfileId = profiles[1].Id
        },
        new UserProfilePhoto
        {
          Url = "https://via.placeholder.com/1000/ebebf0/ffffff?text=3",
          ProfileId = profiles[2].Id
        }
      };

      context.UserProfilePhotos.AddRange(photos);
      context.SaveChanges();
    }

    public static void SeedDrinks(SkelvyContext context)
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

    public static void SeedMeetingRequests(SkelvyContext context)
    {
      if (context.MeetingRequests.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var drinks = context.Drinks.ToList();

      var requests = new[]
      {
        new MeetingRequest
        {
          Status = MeetingStatusTypes.Searching,
          MinDate = DateTime.Now.Date,
          MaxDate = DateTime.Now.AddDays(2).Date,
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

    public static void SeedMeetings(SkelvyContext context)
    {
      if (context.Meetings.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var drinks = context.Drinks.ToList();

      var requests = new[]
      {
        new MeetingRequest
        {
          Status = MeetingStatusTypes.Found,
          MinDate = DateTime.Now.AddDays(2).Date,
          MaxDate = DateTime.Now.AddDays(4).Date,
          MinAge = 18,
          MaxAge = 25,
          Latitude = 1,
          Longitude = 1,
          UserId = users[1].Id
        },
        new MeetingRequest
        {
          Status = MeetingStatusTypes.Found,
          MinDate = DateTime.Now.AddDays(2).Date,
          MaxDate = DateTime.Now.AddDays(4).Date,
          MinAge = 18,
          MaxAge = 25,
          Latitude = 1,
          Longitude = 1,
          UserId = users[2].Id
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
        },
        new MeetingRequestDrink
        {
          MeetingRequestId = requests[1].Id,
          DrinkId = drinks[0].Id
        }
      };

      context.MeetingRequestDrinks.AddRange(requestsDrinks);

      var meetings = new[]
      {
        new Meeting
        {
          Date = DateTime.Now.AddDays(3).Date,
          Latitude = 1,
          Longitude = 1,
          DrinkId = drinks[0].Id
        }
      };

      context.Meetings.AddRange(meetings);
      context.SaveChanges();

      var meetingUsers = new[]
      {
        new MeetingUser
        {
          MeetingId = meetings[0].Id,
          UserId = users[1].Id
        },
        new MeetingUser
        {
          MeetingId = meetings[0].Id,
          UserId = users[2].Id
        }
      };

      context.MeetingUsers.AddRange(meetingUsers);
      context.SaveChanges();
    }
  }
}
