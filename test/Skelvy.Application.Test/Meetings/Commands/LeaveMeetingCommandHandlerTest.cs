using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class LeaveMeetingCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public LeaveMeetingCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new LeaveMeetingCommand(2);
      var handler = new LeaveMeetingCommandHandler(MeetingUsersRepository(), _notifications.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new LeaveMeetingCommand(2);
      var handler = new LeaveMeetingCommandHandler(MeetingUsersRepository(false), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
