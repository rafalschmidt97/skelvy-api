using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Relations.Commands.InviteFriend;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Commands
{
  public class InviteFriendCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public InviteFriendCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new InviteFriendCommand(1, 3);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendCommandHandler(
        new RelationsRepository(dbContext),
        new FriendRequestsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new InviteFriendCommand(10, 3);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendCommandHandler(
        new RelationsRepository(dbContext),
        new FriendRequestsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingRelatedUser()
    {
      var request = new InviteFriendCommand(1, 10);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendCommandHandler(
        new RelationsRepository(dbContext),
        new FriendRequestsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRelation()
    {
      var request = new InviteFriendCommand(2, 3);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendCommandHandler(
        new RelationsRepository(dbContext),
        new FriendRequestsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRequestCreated()
    {
      var request = new InviteFriendCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendCommandHandler(
        new RelationsRepository(dbContext),
        new FriendRequestsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRequestReceived()
    {
      var request = new InviteFriendCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendCommandHandler(
        new RelationsRepository(dbContext),
        new FriendRequestsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }
  }
}
