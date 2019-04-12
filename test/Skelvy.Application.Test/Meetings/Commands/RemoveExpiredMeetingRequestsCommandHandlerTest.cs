using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingRequestsCommandHandlerTest : RequestTestBase
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
      var handler = new RemoveExpiredMeetingRequestsCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request);
    }
  }
}
