using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveMeetingRequestCommand { UserId = 1 };
      var handler = new RemoveMeetingRequestCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = new RemoveMeetingRequestCommand { UserId = 2 };
      var handler = new RemoveMeetingRequestCommandHandler(InitializedDbContext());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RemoveMeetingRequestCommand { UserId = 1 };
      var handler = new RemoveMeetingRequestCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
