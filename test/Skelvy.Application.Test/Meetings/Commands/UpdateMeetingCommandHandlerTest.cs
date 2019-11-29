using System;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.UpdateMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class UpdateMeetingCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldUpdateRequest()
    {
      var request = new UpdateMeetingCommand(2, 1, DateTimeOffset.UtcNow, 1, 1, 4, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new UpdateMeetingCommand(100, 1, DateTimeOffset.UtcNow, 1, 1, 4, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new UpdateMeetingCommand(2, 100, DateTimeOffset.UtcNow, 1, 1, 4, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidActivity()
    {
      var request = new UpdateMeetingCommand(2, 1, DateTimeOffset.UtcNow, 1, 1, 4, 100, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
