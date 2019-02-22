using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class LeaveMeetingCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new LeaveMeetingCommand { UserId = 2 };
      var handler = new LeaveMeetingCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new LeaveMeetingCommand { UserId = 2 };
      var handler = new LeaveMeetingCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
