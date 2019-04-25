using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests;
using Skelvy.Application.Notifications;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingRequestsCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public RemoveExpiredMeetingRequestsCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingRequestsCommand();
      var handler = new RemoveExpiredMeetingRequestsCommandHandler(MeetingRequestsRepository(), _notifications.Object);

      await handler.Handle(request);
    }
  }
}
