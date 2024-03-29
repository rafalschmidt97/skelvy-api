using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.UpdateMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class UpdateMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public UpdateMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldUpdateRequest()
    {
      var request = new UpdateMeetingCommand(2, 1, DateTimeOffset.UtcNow, 1, 1, 4, null, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new UpdateMeetingCommand(100, 1, DateTimeOffset.UtcNow, 1, 1, 4, null, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new UpdateMeetingCommand(2, 100, DateTimeOffset.UtcNow, 1, 1, 4, null, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidActivity()
    {
      var request = new UpdateMeetingCommand(2, 1, DateTimeOffset.UtcNow, 1, 1, 4, null, 100, false);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
