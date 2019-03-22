using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands.AddMeetingChatMessage;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMeetingChatMessageCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public AddMeetingChatMessageCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddMeetingChatMessageCommand { Message = "Hello World", UserId = 2 };
      var handler = new AddMeetingChatMessageCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new AddMeetingChatMessageCommand { Message = "Hello World", UserId = 2 };
      var handler = new AddMeetingChatMessageCommandHandler(DbContext(), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
