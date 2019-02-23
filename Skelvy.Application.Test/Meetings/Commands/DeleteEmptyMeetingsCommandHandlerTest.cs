using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.DeleteEmptyMeetings;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class DeleteEmptyMeetingsCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new DeleteEmptyMeetingsCommand();
      var handler = new DeleteEmptyMeetingsCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
