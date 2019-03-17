using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Application.Users.Commands.AddUserDevice;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class AddUserDeviceCommandHandlerTest : RequestTestBase
  {
    private const string RegistrationId = "ABC";
    private readonly Mock<IPushNotificationsService> _notifications;

    public AddUserDeviceCommandHandlerTest()
    {
      _notifications = new Mock<IPushNotificationsService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddUserDeviceCommand { UserId = 1, RegistrationId = RegistrationId };
      _notifications
        .Setup(x => x.VerifyIds(It.IsAny<ICollection<string>>(), CancellationToken.None))
        .ReturnsAsync(new PushVerification { Success = 1 });
      var handler = new AddUserDeviceCommandHandler(InitializedDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
      var request = new AddUserDeviceCommand { UserId = 1, RegistrationId = RegistrationId };
      var handler = new AddUserDeviceCommandHandler(DbContext(), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowConflictException()
    {
      var request = new AddUserDeviceCommand { UserId = 1, RegistrationId = RegistrationId };
      var handler = new AddUserDeviceCommandHandler(InitializedDbContext(), _notifications.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
