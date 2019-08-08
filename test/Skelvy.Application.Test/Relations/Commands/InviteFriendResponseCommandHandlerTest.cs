using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Relations.Commands.InviteFriendResponse;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Commands
{
  public class InviteFriendResponseCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public InviteFriendResponseCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowExceptionOnAccept()
    {
      var request = new InviteFriendResponseCommand(2, 1, true);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendResponseCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldNotThrowExceptionOnDeny()
    {
      var request = new InviteFriendResponseCommand(2, 1, false);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendResponseCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new InviteFriendResponseCommand(10, 1, true);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendResponseCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingRequest()
    {
      var request = new InviteFriendResponseCommand(1, 10, true);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendResponseCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonUserRequest()
    {
      var request = new InviteFriendResponseCommand(4, 1, true);
      var dbContext = InitializedDbContext();
      var handler = new InviteFriendResponseCommandHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }
  }
}
