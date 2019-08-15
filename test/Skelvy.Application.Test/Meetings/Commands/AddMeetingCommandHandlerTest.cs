using System;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.AddMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMeetingCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = new AddMeetingCommand(1, DateTimeOffset.UtcNow, 1, 1, 1);
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
      var request = new AddMeetingCommand(100, DateTimeOffset.UtcNow, 1, 1, 1);
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
      var request = new AddMeetingCommand(100, DateTimeOffset.UtcNow, 1, 1, 100);
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
  }
}
