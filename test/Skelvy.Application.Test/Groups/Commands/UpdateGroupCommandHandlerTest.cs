using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Groups.Commands.UpdateGroup;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Groups.Commands
{
  public class UpdateGroupCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public UpdateGroupCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldUpdateRequest()
    {
      var request = new UpdateGroupCommand(2, 1, "Example");
      var dbContext = InitializedDbContext();
      var handler = new UpdateGroupCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new UpdateGroupCommand(100, 1, "Example");
      var dbContext = InitializedDbContext();
      var handler = new UpdateGroupCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidGroup()
    {
      var request = new UpdateGroupCommand(2, 100, "Example");
      var dbContext = InitializedDbContext();
      var handler = new UpdateGroupCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
