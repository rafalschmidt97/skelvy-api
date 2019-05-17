using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.AddBlockedUser;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class AddBlockedUserCommandHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddBlockedUserCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedUserCommandHandler(
        new BlockedUsersRepository(dbContext),
        new UsersRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowConflictException()
    {
      var request = new AddBlockedUserCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedUserCommandHandler(
        new BlockedUsersRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
      var request = new AddBlockedUserCommand(1, 2);
      var dbContext = DbContext();
      var handler = new AddBlockedUserCommandHandler(
        new BlockedUsersRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
