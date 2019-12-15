using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Relations.Commands.RemoveFriend;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Commands
{
  public class RemoveFriendCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public RemoveFriendCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RemoveFriendCommand(2, 3);
      var dbContext = InitializedDbContext();
      var handler = new RemoveFriendCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new RemoveFriendCommand(10, 1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveFriendCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingRelation()
    {
      var request = new RemoveFriendCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveFriendCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
