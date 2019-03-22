using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Application.Users.Commands.RemoveUser;
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
      var request = new RemoveUserCommand { Id = 1 };
      var handler = new RemoveUserCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RemoveUserCommand { Id = 1 };
      var handler = new RemoveUserCommandHandler(DbContext(), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
