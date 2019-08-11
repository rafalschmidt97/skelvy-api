using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.UpdateUserName;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class UpdateUserNameCommandHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new UpdateUserNameCommand(1, "example");
      var handler = new UpdateUserNameCommandHandler(UsersRepository());

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new UpdateUserNameCommand(1, "example");
      var handler = new UpdateUserNameCommandHandler(UsersRepository(false));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingUserName()
    {
      var request = new UpdateUserNameCommand(1, "user.2");
      var handler = new UpdateUserNameCommandHandler(UsersRepository());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }
  }
}
