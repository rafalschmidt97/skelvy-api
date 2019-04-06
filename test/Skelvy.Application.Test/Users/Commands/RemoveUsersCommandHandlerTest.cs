using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.RemoveUsers;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class RemoveUsersCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveUsersCommand();
      var handler = new RemoveUsersCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }
  }
}
