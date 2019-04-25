using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Commands.MatchMeetingRequests;
using Skelvy.Application.Notifications;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class MatchMeetingRequestsCommandHandlerTest : DatabaseRequestTestBase
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
      var handler = new MatchMeetingRequestsCommandHandler(MeetingRequestsRepository(), _notifications.Object);

      await handler.Handle(request);
    }
  }
}
