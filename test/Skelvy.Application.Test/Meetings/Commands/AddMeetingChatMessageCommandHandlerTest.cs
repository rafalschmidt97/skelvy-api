using System;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Commands.AddMeetingChatMessage;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMeetingChatMessageCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public AddMeetingChatMessageCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddMeetingChatMessageCommand(DateTimeOffset.UtcNow, "Hello World", 2);
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingChatMessageCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        _notifications.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new AddMeetingChatMessageCommand(DateTimeOffset.UtcNow, "Hello World", 2);
      var dbContext = DbContext();
      var handler = new AddMeetingChatMessageCommandHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
