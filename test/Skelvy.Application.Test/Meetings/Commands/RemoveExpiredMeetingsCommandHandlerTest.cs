using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingsCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public RemoveExpiredMeetingsCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveExpiredMeetingsCommand();
      var handler = new RemoveExpiredMeetingsCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request);
    }
  }
}
