using System.Threading.Tasks;
using Skelvy.Application.Relations.Commands.RemoveBlocked;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Commands
{
  public class RemoveBlockedCommandHandlerTest : DatabaseRequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveBlockedCommand(2, 4);
      var dbContext = InitializedDbContext();
      var handler = new RemoveBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new RemoveBlockedCommand(10, 1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingRelation()
    {
      var request = new RemoveBlockedCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
