using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Exceptions.Extra;
using Skelvy.Application.Core.Infrastructure;
using Skelvy.Application.Users.Commands.CreateUser;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class CreateUserCommandHandlerTest : RequestTestBase
  {
    private const string UserEmail = "user@gmail.com";
    private const string UserName = "User";

    [Fact]
    public async Task ShouldReturnUserId()
    {
      var request = new CreateUserCommand { Email = UserEmail, Name = UserName };
      var notificationService = new Mock<INotificationService>();
      notificationService.Setup(x => x.Send(It.IsAny<string>())).Returns(Task.CompletedTask);
      // We can also create handler in constructor if db is only passed to simplify code.
      // But the context will need to be Dispose. It might be cool if all test will need to have initialized database.
      var handler = new CreateUserCommandHandler(DbContext(), notificationService.Object);

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.Equal(1, result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new CreateUserCommand { Email = UserEmail, Name = UserName };
      var notificationService = new Mock<INotificationService>();
      notificationService.Setup(x => x.Send(It.IsAny<string>())).Returns(Task.CompletedTask);
      var handler = new CreateUserCommandHandler(DbContext(), notificationService.Object);

      await handler.Handle(request, CancellationToken.None);

      await Assert.ThrowsAsync<AlreadyExistsException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
