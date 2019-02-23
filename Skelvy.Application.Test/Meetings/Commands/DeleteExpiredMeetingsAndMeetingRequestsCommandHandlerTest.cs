using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.DeleteExpiredMeetingsAndMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class DeleteExpiredMeetingsAndMeetingRequestsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new DeleteExpiredMeetingsAndMeetingRequestsCommand();
      var handler = new DeleteExpiredMeetingsAndMeetingRequestsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
