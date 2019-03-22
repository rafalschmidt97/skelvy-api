using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands.MatchMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class MatchMeetingRequestsCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public MatchMeetingRequestsCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new MatchMeetingRequestsCommand();
      var handler = new MatchMeetingRequestsCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
