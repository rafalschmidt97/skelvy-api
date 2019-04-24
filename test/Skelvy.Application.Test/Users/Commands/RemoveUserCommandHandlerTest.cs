using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class RemoveUserCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public RemoveUserCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveUserCommand(1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveUserCommandHandler(
        new UsersRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RemoveUserCommand(1);
      var dbContext = DbContext();
      var handler = new RemoveUserCommandHandler(
        new UsersRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
