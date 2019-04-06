using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Users.Commands.DisableUser;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class DisableUserCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public DisableUserCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new DisableUserCommand { Id = 1, Reason = "XYZ" };
      var handler = new DisableUserCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
      var request = new DisableUserCommand { Id = 1, Reason = "XYZ" };
      var handler = new DisableUserCommandHandler(DbContext(), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
