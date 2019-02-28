using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.RemoveEmptyMeetings;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveEmptyMeetingsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveEmptyMeetingsCommand();
      var handler = new RemoveEmptyMeetingsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
