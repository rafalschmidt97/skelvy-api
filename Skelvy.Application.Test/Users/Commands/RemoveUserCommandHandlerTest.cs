using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Users.Commands.RemoveUser;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class RemoveUserCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveUserCommand { Id = 1 };
      var handler = new RemoveUserCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RemoveUserCommand { Id = 1 };
      var handler = new RemoveUserCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
