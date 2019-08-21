using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class RemoveUserCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public RemoveUserCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowExceptionWithUserWithoutMeeting()
    {
      var request = new RemoveUserCommand(1);
      var dbContext = InitializedDbContext();
      var handler = new RemoveUserCommandHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldNotThrowExceptionWithUserWithMeeting()
    {
      var request = new RemoveUserCommand(2);
      var dbContext = InitializedDbContext();
      var handler = new RemoveUserCommandHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RemoveUserCommand(1);
      var dbContext = DbContext();
      var handler = new RemoveUserCommandHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new FriendInvitationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
