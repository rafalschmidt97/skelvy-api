using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingRequestsCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public RemoveExpiredMeetingRequestsCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingRequestsCommand();
      var handler = new RemoveExpiredMeetingRequestsCommandHandler(MeetingRequestsRepository(), _mediator.Object);

      await handler.Handle(request);
    }
  }
}
