using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Commands.MatchMeetingRequests;
using Skelvy.Application.Notifications;
using Skelvy.Persistence.Repositories;
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
      var dbContext = InitializedDbContext();
      var handler = new MatchMeetingRequestsCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await handler.Handle(request);
    }
  }
}
