using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.RemoveMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public RemoveMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveMeetingCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new RemoveMeetingCommand(100, 2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new RemoveMeetingCommand(1, 100);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithUserWithoutPermission()
    {
      var request = new RemoveMeetingCommand(1, 3);
      var dbContext = InitializedDbContext();
      var handler = new RemoveMeetingCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingInvitationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
