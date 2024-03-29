using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Groups.Commands.LeaveGroup;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Groups.Commands
{
  public class LeaveGroupCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public LeaveGroupCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldLeaveGroup()
    {
      var request = new LeaveGroupCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new LeaveGroupCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingGroupUser()
    {
      var request = new LeaveGroupCommand(2, 10);
      var dbContext = InitializedDbContext();
      var handler = new LeaveGroupCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = new LeaveGroupCommand(1, 2);
      var dbContext = InitializedDbContext();
      var handler = new LeaveGroupCommandHandler(
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }
  }
}
