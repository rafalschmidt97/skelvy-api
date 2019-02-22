using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Meetings.Commands.DeleteMeetingRequest;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class DeleteMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new DeleteMeetingRequestCommand { UserId = 1 };
      var handler = new DeleteMeetingRequestCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = new DeleteMeetingRequestCommand { UserId = 2 };
      var handler = new DeleteMeetingRequestCommandHandler(InitializedDbContext());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new DeleteMeetingRequestCommand { UserId = 1 };
      var handler = new DeleteMeetingRequestCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
