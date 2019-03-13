using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingsCommand();
      var handler = new RemoveExpiredMeetingsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
