using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Relations.Commands.AddBlocked;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Commands
{
  public class AddBlockedCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public AddBlockedCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new AddBlockedCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new AddBlockedCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

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
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

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
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

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
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }
  }
}
