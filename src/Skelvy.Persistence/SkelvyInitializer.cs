using System;
using System.Globalization;
using System.Linq;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Persistence
{
  public static class SkelvyInitializer
  {
    public static void Initialize(SkelvyContext context)
    {
      SeedUsers(context);
      SeedProfiles(context);
      SeedFriendInvitations(context);
      SeedRelations(context);
      SeedActivities(context);
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
        new User("user1@gmail.com", "user.1", LanguageType.EN),
        new User("user2@gmail.com", "user.2", LanguageType.EN),
        new User("user3@gmail.com", "user.3", LanguageType.EN),
        new User("user4@gmail.com", "user.4", LanguageType.EN),
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
      if (context.Profiles.Any())
      {
        return;
      }

      var users = context.Users.ToList();

      var profiles = new[]
      {
        new Profile(
          "User1",
          DateTimeOffset.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderType.Male,
          users[0].Id),
        new Profile(
          "User2",
          DateTimeOffset.ParseExact("22/04/1996", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderType.Male,
          users[1].Id),
        new Profile(
          "User3",
          DateTimeOffset.ParseExact("22/04/1995", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderType.Male,
          users[2].Id),
        new Profile(
          "User4",
          DateTimeOffset.ParseExact("22/04/1997", "dd/MM/yyyy", CultureInfo.CurrentCulture).ToUniversalTime(),
          GenderType.Male,
          users[3].Id),
      };

      context.Profiles.AddRange(profiles);
      context.SaveChanges();

      var attachments = new[]
      {
        new Attachment(AttachmentType.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=1"),
        new Attachment(AttachmentType.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=2"),
        new Attachment(AttachmentType.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=3"),
        new Attachment(AttachmentType.Image, "https://via.placeholder.com/1000/ebebf0/ffffff?text=4"),
      };

      context.Attachments.AddRange(attachments);
      context.SaveChanges();

      var photos = new[]
      {
        new ProfilePhoto(attachments[0].Id, 1, profiles[0].Id),
        new ProfilePhoto(attachments[1].Id, 1, profiles[1].Id),
        new ProfilePhoto(attachments[2].Id, 1, profiles[2].Id),
        new ProfilePhoto(attachments[3].Id, 1, profiles[3].Id),
      };

      context.ProfilePhotos.AddRange(photos);
      context.SaveChanges();
    }

    public static void SeedFriendInvitations(SkelvyContext context)
    {
      if (context.FriendInvitations.Any())
      {
        return;
      }

      var invitations = new[]
      {
        new FriendInvitation(1, 2),
      };

      context.FriendInvitations.AddRange(invitations);
      context.SaveChanges();
    }

    public static void SeedRelations(SkelvyContext context)
    {
      if (context.Relations.Any())
      {
        return;
      }

      var relations = new[]
      {
        new Relation(2, 3, RelationType.Friend),
        new Relation(3, 2, RelationType.Friend),
        new Relation(2, 4, RelationType.Blocked),
      };

      context.Relations.AddRange(relations);
      context.SaveChanges();
    }

    public static void SeedActivities(SkelvyContext context)
    {
      if (context.Activities.Any())
      {
        return;
      }

      var activities = new[]
      {
        new Activity("Soft drinks", ActivityType.Party, 4, 15),
        new Activity("Alcoholic drinks", ActivityType.Party, 4, 15),
        new Activity("Board games", ActivityType.Party, 4, 15),
        new Activity("Running", ActivityType.Sport, 4, 10),
        new Activity("Cycling", ActivityType.Sport, 4, 20),
        new Activity("Motorcycling", ActivityType.Sport, 4, 30),
        new Activity("Football", ActivityType.Sport, 12, 15),
        new Activity("Basketball", ActivityType.Sport, 12, 15),
        new Activity("Volleyball", ActivityType.Sport, 12, 15),
        new Activity("Tennis", ActivityType.Sport, 2, 15),
        new Activity("Squash", ActivityType.Sport, 2, 15),
      };

      context.Activities.AddRange(activities);
      context.SaveChanges();
    }

    public static void SeedMeetingRequests(SkelvyContext context)
    {
      if (context.MeetingRequests.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var activities = context.Activities.ToList();

      var requests = new[]
      {
        new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, users[0].Id),
      };

      context.MeetingRequests.AddRange(requests);
      context.SaveChanges();

      var requestActivities = new[]
      {
        new MeetingRequestActivity(requests[0].Id, activities[0].Id),
      };

      context.MeetingRequestActivities.AddRange(requestActivities);
      context.SaveChanges();
    }

    public static void SeedMeetings(SkelvyContext context)
    {
      if (context.Meetings.Any())
      {
        return;
      }

      var users = context.Users.ToList();
      var activities = context.Activities.ToList();

      var requests = new[]
      {
        new MeetingRequest(DateTimeOffset.UtcNow.AddDays(2), DateTimeOffset.UtcNow.AddDays(4), 18, 25, 1, 1, users[1].Id),
        new MeetingRequest(DateTimeOffset.UtcNow.AddDays(2), DateTimeOffset.UtcNow.AddDays(4), 18, 25, 1, 1, users[2].Id),
      };

      requests[0].MarkAsFound();
      requests[1].MarkAsFound();

      context.MeetingRequests.AddRange(requests);
      context.SaveChanges();

      var requestActivities = new[]
      {
        new MeetingRequestActivity(requests[0].Id, activities[0].Id),
        new MeetingRequestActivity(requests[1].Id, activities[0].Id),
      };

      context.MeetingRequestActivities.AddRange(requestActivities);

      var groups = new[]
      {
        new Group(),
        new Group(),
      };

      context.Groups.AddRange(groups);
      context.SaveChanges();

      var meetings = new[]
      {
        new Meeting(DateTimeOffset.UtcNow.AddDays(3), 1, 1, activities[0].Size, false, false, groups[0].Id, activities[0].Id),
      };

      context.Meetings.AddRange(meetings);
      context.SaveChanges();

      var groupUsers = new[]
      {
        new GroupUser(groups[0].Id, users[1].Id, requests[0].Id, GroupUserRoleType.Owner),
        new GroupUser(groups[0].Id, users[2].Id, requests[1].Id, GroupUserRoleType.Admin),
        new GroupUser(groups[1].Id, users[0].Id, GroupUserRoleType.Admin),
        new GroupUser(groups[1].Id, users[3].Id, GroupUserRoleType.Admin),
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
        new Message(MessageType.Response, DateTimeOffset.UtcNow.AddHours(-2), "Hello User3", null, null, users[1].Id, meetings[0].Id),
        new Message(MessageType.Action, DateTimeOffset.UtcNow.AddHours(-1), null, null, MessageActionType.Seen, users[2].Id, meetings[0].Id),
      };

      context.Messages.AddRange(messages);
      context.SaveChanges();
    }
  }
}
