using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings;
using Skelvy.Application.Notifications;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveExpiredMeetingsCommandHandlerTest : DatabaseRequestTestBase
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
      var dbContext = InitializedDbContext();
      var handler = new RemoveExpiredMeetingsCommandHandler(
        new MeetingsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await handler.Handle(request);
    }
  }
}
