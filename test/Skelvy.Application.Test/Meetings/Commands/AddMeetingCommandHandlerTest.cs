using System;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.AddMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMeetingCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = new AddMeetingCommand(1, DateTimeOffset.UtcNow, 1, 1, 4, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new AddMeetingCommand(100, DateTimeOffset.UtcNow, 1, 1, 4, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidActivity()
    {
      var request = new AddMeetingCommand(100, DateTimeOffset.UtcNow, 1, 1, 4, 100);
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithTooManyMeetings()
    {
      var request = new AddMeetingCommand(1, DateTimeOffset.UtcNow, 1, 1, 4, 1);
      var dbContext = TestDbContextWithThreeMeetings();
      var handler = new AddMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContextWithThreeMeetings()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedActivities(context);

      var groups = new[]
      {
        new Group(),
        new Group(),
        new Group(),
      };

      context.Groups.AddRange(groups);
      context.SaveChanges();

      var meetings = new[]
      {
        new Meeting(DateTimeOffset.UtcNow.AddDays(1), 1, 1, 4, true, false, groups[0].Id, 1),
        new Meeting(DateTimeOffset.UtcNow.AddDays(2), 2, 2, 4, true, false, groups[1].Id, 2),
        new Meeting(DateTimeOffset.UtcNow.AddDays(3), 3, 3, 4, true, false, groups[2].Id, 3),
      };

      context.Meetings.AddRange(meetings);
      context.SaveChanges();

      var groupUsers = new[]
      {
        new GroupUser(groups[0].Id, 1, GroupUserRoleType.Owner),
        new GroupUser(groups[1].Id, 1, GroupUserRoleType.Owner),
        new GroupUser(groups[2].Id, 1, GroupUserRoleType.Owner),
      };

      context.GroupUsers.AddRange(groupUsers);

      context.SaveChanges();

      return context;
    }
  }
}
