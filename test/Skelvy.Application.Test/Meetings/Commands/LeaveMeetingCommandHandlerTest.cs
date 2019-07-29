using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class LeaveMeetingCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public LeaveMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new LeaveMeetingCommand(2);
      var dbContext = InitializedDbContext();
      var handler = new LeaveMeetingCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new LeaveMeetingCommand(2);
      var dbContext = DbContext();
      var handler = new LeaveMeetingCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
