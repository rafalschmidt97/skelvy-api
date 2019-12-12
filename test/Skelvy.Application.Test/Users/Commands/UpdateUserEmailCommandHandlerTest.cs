using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.UpdateUserEmail;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class UpdateUserEmailCommandHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new UpdateUserEmailCommand(1, "example@gmail.com");
      var handler = new UpdateUserEmailCommandHandler(UsersRepository());

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new UpdateUserEmailCommand(1, "example@gmail.com");
      var handler = new UpdateUserEmailCommandHandler(UsersRepository(false));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
