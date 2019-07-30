using System;
using System.Globalization;
using System.Linq;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Attachments;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Persistence
{
  public static class SkelvyInitializer
  {
    public static void Initialize(SkelvyContext context)
    {
      SeedUsers(context);
      SeedProfiles(context);
      SeedBlockedUsers(context);
      SeedDrinkTypes(context);
      SeedMeetingRequests(context);
      SeedMeetings(context);
      SeedMessages(context);
    }

    public static void SeedUsers(SkelvyContext context)
    {
      if (context.Users.Any())
      {
        return;
      }

      var users = new[]
      {
        new User("user1@gmail.com", LanguageTypes.EN),
        new User("user2@gmail.com", LanguageTypes.EN),
        new User("user3@gmail.com", LanguageTypes.EN),
        new User("user4@gmail.com", LanguageTypes.EN),
      };

      users[0].RegisterFacebook("facebook1");
      users[0].RegisterGoogle("google1");
      users[1].RegisterFacebook("facebook2");
      users[1].RegisterGoogle("google2");
      users[2].RegisterFacebook("facebook3");
      users[2].RegisterGoogle("google3");
      users[3].RegisterFacebook("facebook4");
      users[3].RegisterGoogle("google4");

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
        new UserProfile(
          "User1",
          DateTimeOffset.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderTypes.Male,
          users[0].Id),
        new UserProfile(
          "User2",
          DateTimeOffset.ParseExact("22/04/1996", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderTypes.Male,
          users[1].Id),
        new UserProfile(
          "User3",
          DateTimeOffset.ParseExact("22/04/1995", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderTypes.Male,
          users[2].Id),
        new UserProfile(
          "User4",
          DateTimeOffset.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderTypes.Male,
          users[3].Id),
      };

      context.UserProfiles.AddRange(profiles);
      context.SaveChanges();

      var attachments = new[]
      {
        new Attachment(AttachmentTypes.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=1"),
        new Attachment(AttachmentTypes.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=2"),
        new Attachment(AttachmentTypes.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=3"),
        new Attachment(AttachmentTypes.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=4"),
      };

      context.Attachments.AddRange(attachments);
      context.SaveChanges();

      var photos = new[]
      {
        new UserProfilePhoto(attachments[0].Id, 1, profiles[0].Id),
        new UserProfilePhoto(attachments[1].Id, 1, profiles[1].Id),
        new UserProfilePhoto(attachments[2].Id, 1, profiles[2].Id),
        new UserProfilePhoto(attachments[3].Id, 1, profiles[3].Id),
      };

      context.UserProfilePhotos.AddRange(photos);
      context.SaveChanges();
    }

    public static void SeedBlockedUsers(SkelvyContext context)
    {
      if (context.BlockedUsers.Any())
      {
        return;
      }

      var users = new[]
      {
        new BlockedUser(2, 1),
      };

      context.BlockedUsers.AddRange(users);
      context.SaveChanges();
    }

    public static void SeedDrinkTypes(SkelvyContext context)
    {
      if (context.DrinkTypes.Any())
      {
        return;
      }

      var drinkTypes = new[]
      {
        new DrinkType("soft drinks"),
        new DrinkType("alcoholic drinks"),
      };

      context.DrinkTypes.AddRange(drinkTypes);
      context.SaveChanges();
    }

    public static void SeedMeetingRequests(SkelvyContext context)
    {
      if (context.MeetingRequests.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var drinkTypes = context.DrinkTypes.ToList();

      var requests = new[]
      {
        new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, users[0].Id),
      };

      context.MeetingRequests.AddRange(requests);
      context.SaveChanges();

      var requestsDrinkTypes = new[]
      {
        new MeetingRequestDrinkType(requests[0].Id, drinkTypes[0].Id),
      };

      context.MeetingRequestDrinkTypes.AddRange(requestsDrinkTypes);
      context.SaveChanges();
    }

    public static void SeedMeetings(SkelvyContext context)
    {
      if (context.Meetings.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var drinkTypes = context.DrinkTypes.ToList();

      var requests = new[]
      {
        new MeetingRequest(DateTimeOffset.UtcNow.AddDays(2), DateTimeOffset.UtcNow.AddDays(4), 18, 25, 1, 1, users[1].Id),
        new MeetingRequest(DateTimeOffset.UtcNow.AddDays(2), DateTimeOffset.UtcNow.AddDays(4), 18, 25, 1, 1, users[2].Id),
      };

      requests[0].MarkAsFound();
      requests[1].MarkAsFound();

      context.MeetingRequests.AddRange(requests);
      context.SaveChanges();

      var requestsDrinkTypes = new[]
      {
        new MeetingRequestDrinkType(requests[0].Id, drinkTypes[0].Id),
        new MeetingRequestDrinkType(requests[1].Id, drinkTypes[0].Id),
      };

      context.MeetingRequestDrinkTypes.AddRange(requestsDrinkTypes);

      var groups = new[]
      {
        new Group(),
      };

      context.Groups.AddRange(groups);
      context.SaveChanges();

      var meetings = new[]
      {
        new Meeting(DateTimeOffset.UtcNow.AddDays(3), 1, 1, groups[0].Id, drinkTypes[0].Id),
      };

      context.Meetings.AddRange(meetings);
      context.SaveChanges();

      var groupUsers = new[]
      {
        new GroupUser(groups[0].Id, users[1].Id, requests[0].Id),
        new GroupUser(groups[0].Id, users[2].Id, requests[1].Id),
      };

      context.GroupUsers.AddRange(groupUsers);
      context.SaveChanges();
    }

    public static void SeedMessages(SkelvyContext context)
    {
      if (context.Messages.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var meetings = context.Users.ToList();

      var messages = new[]
      {
        new Message(DateTimeOffset.UtcNow.AddHours(-2), "Hello User3", null, users[1].Id, meetings[0].Id),
        new Message(DateTimeOffset.UtcNow.AddHours(-1), "Hello User2", null, users[2].Id, meetings[0].Id),
      };

      context.Messages.AddRange(messages);
      context.SaveChanges();
    }
  }
}
