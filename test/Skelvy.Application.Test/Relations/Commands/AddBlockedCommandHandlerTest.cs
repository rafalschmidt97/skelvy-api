using System.Threading.Tasks;
using Skelvy.Application.Relations.Commands.AddBlocked;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Commands
{
  public class AddBlockedCommandHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddBlockedCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        new FriendInvitationsRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new AddBlockedCommand(10, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        new FriendInvitationsRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingRelatedUser()
    {
      var request = new AddBlockedCommand(2, 10);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        new FriendInvitationsRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRelation()
    {
      var request = new AddBlockedCommand(2, 4);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        new FriendInvitationsRepository(dbContext));

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }
  }
}
