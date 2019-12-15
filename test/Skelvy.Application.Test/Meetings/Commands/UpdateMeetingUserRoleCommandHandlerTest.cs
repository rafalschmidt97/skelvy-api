using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.UpdateMeetingUserRole;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class UpdateMeetingUserRoleCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public UpdateMeetingUserRoleCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = new UpdateMeetingUserRoleCommand(2, 1, 3, GroupUserRoleType.Member);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingUserRoleCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new UpdateMeetingUserRoleCommand(2, 100, 3, GroupUserRoleType.Member);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingUserRoleCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new UpdateMeetingUserRoleCommand(100, 1, 3, GroupUserRoleType.Member);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingUserRoleCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUpdatedUser()
    {
      var request = new UpdateMeetingUserRoleCommand(2, 1, 100, GroupUserRoleType.Member);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingUserRoleCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithoutPermission()
    {
      var request = new UpdateMeetingUserRoleCommand(3, 1, 2, GroupUserRoleType.Member);
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingUserRoleCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ForbiddenException>(() =>
        handler.Handle(request));
    }
  }
}
