using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.MatchMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class MatchMeetingRequestsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new MatchMeetingRequestsCommand();
      var handler = new MatchMeetingRequestsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
