using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingRequestsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingRequestsCommand();
      var handler = new RemoveExpiredMeetingRequestsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
