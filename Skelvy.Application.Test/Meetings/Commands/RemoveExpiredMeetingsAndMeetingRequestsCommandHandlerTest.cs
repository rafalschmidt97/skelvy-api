using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingsAndMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingsAndMeetingRequestsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingsAndMeetingRequestsCommand();
      var handler = new RemoveExpiredMeetingsAndMeetingRequestsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
