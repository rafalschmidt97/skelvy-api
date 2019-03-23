using System;
using System.Globalization;
using System.Linq;
using Skelvy.Application.Meetings.Commands;
using Skelvy.Application.Users.Commands;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Initializers
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
      SeedMeetingsChatMessages(context);
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
          Birthday = DateTimeOffset.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          Gender = GenderTypes.Male,
          UserId = users[0].Id
        },
        new UserProfile
        {
          Name = "User2",
          Birthday = DateTimeOffset.ParseExact("22/04/1996", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          Gender = GenderTypes.Male,
          UserId = users[1].Id
        },
        new UserProfile
        {
          Name = "User3",
          Birthday = DateTimeOffset.ParseExact("22/04/1995", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
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
          MinDate = DateTimeOffset.UtcNow,
          MaxDate = DateTimeOffset.UtcNow,
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
          MinDate = DateTimeOffset.UtcNow.AddDays(2),
          MaxDate = DateTimeOffset.UtcNow.AddDays(4),
          MinAge = 18,
          MaxAge = 25,
          Latitude = 1,
          Longitude = 1,
          UserId = users[1].Id
        },
        new MeetingRequest
        {
          Status = MeetingStatusTypes.Found,
          MinDate = DateTimeOffset.UtcNow.AddDays(2),
          MaxDate = DateTimeOffset.UtcNow.AddDays(4),
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
          Date = DateTimeOffset.UtcNow.AddDays(3),
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

    public static void SeedMeetingsChatMessages(SkelvyContext context)
    {
      if (context.MeetingChatMessages.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var meetings = context.Users.ToList();

      var messages = new[]
      {
        new MeetingChatMessage
        {
          MeetingId = meetings[0].Id,
          UserId = users[1].Id,
          Date = DateTimeOffset.UtcNow.AddHours(-2),
          Message = "Hello User3"
        },
        new MeetingChatMessage
        {
          MeetingId = meetings[0].Id,
          UserId = users[2].Id,
          Date = DateTimeOffset.UtcNow.AddHours(-1),
          Message = "Hello User2"
        }
      };

      context.MeetingChatMessages.AddRange(messages);
      context.SaveChanges();
    }
  }
}
