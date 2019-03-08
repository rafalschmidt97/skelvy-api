using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands.RemoveEmptyMeetings;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveEmptyMeetingsCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public RemoveEmptyMeetingsCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveEmptyMeetingsCommand();
      var handler = new RemoveEmptyMeetingsCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
