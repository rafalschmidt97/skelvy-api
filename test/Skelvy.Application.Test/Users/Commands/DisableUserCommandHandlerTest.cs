using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Users.Commands.DisableUser;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class DisableUserCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public DisableUserCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new DisableUserCommand(1, "XYZ");
      var dbContext = InitializedDbContext();
      var handler = new DisableUserCommandHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
      var request = new DisableUserCommand(1, "XYZ");
      var dbContext = DbContext();
      var handler = new DisableUserCommandHandler(
        new UsersRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
