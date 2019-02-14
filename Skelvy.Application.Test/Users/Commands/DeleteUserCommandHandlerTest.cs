using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Users.Commands.DeleteUser;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class DeleteUserCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new DeleteUserCommand { Id = 1 };
      var handler = new DeleteUserCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new DeleteUserCommand { Id = 1 };
      var handler = new DeleteUserCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
