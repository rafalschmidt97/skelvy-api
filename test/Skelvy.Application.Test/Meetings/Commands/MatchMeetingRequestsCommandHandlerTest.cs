using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.MatchMeetingRequests;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class MatchMeetingRequestsCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<MatchMeetingRequestsCommandHandler>> _logger;

    public MatchMeetingRequestsCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
      _logger = new Mock<ILogger<MatchMeetingRequestsCommandHandler>>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new MatchMeetingRequestsCommand();
      var dbContext = InitializedDbContext();
      var handler = new MatchMeetingRequestsCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await handler.Handle(request);
    }
  }
}
