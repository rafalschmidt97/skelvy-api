using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.RemoveBlockedUser;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class RemoveBlockedUserCommandHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveBlockedUserCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveBlockedUserCommandHandler(
        new BlockedUsersRepository(dbContext),
        new UsersRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowNotFoundBlockedUserException()
    {
      var request = new RemoveBlockedUserCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveBlockedUserCommandHandler(
        new BlockedUsersRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowNotFoundUserException()
    {
      var request = new RemoveBlockedUserCommand(1, 2);
      var dbContext = DbContext();
      var handler = new RemoveBlockedUserCommandHandler(
        new BlockedUsersRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
