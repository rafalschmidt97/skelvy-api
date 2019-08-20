using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.InviteToMeetingResponse;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class InviteToMeetingResponseCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public InviteToMeetingResponseCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowExceptionOnAccept()
    {
      var request = new InviteToMeetingResponseCommand(1, 1, true);
      var dbContext = TestDbContextWithFriendRelationAndMeetingInvitation();
      var handler = new InviteToMeetingResponseCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldNotThrowExceptionOnDeny()
    {
      var request = new InviteToMeetingResponseCommand(1, 1, false);
      var dbContext = TestDbContextWithFriendRelationAndMeetingInvitation();
      var handler = new InviteToMeetingResponseCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new InviteToMeetingResponseCommand(100, 1, true);
      var dbContext = TestDbContextWithFriendRelationAndMeetingInvitation();
      var handler = new InviteToMeetingResponseCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingInvitation()
    {
      var request = new InviteToMeetingResponseCommand(1, 100, true);
      var dbContext = TestDbContextWithFriendRelationAndMeetingInvitation();
      var handler = new InviteToMeetingResponseCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidInvitation()
    {
      var request = new InviteToMeetingResponseCommand(4, 100, true);
      var dbContext = TestDbContextWithFriendRelationAndMeetingInvitation();
      var handler = new InviteToMeetingResponseCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingMeeting()
    {
      var request = new InviteToMeetingResponseCommand(1, 1, true);
      var dbContext = TestDbContextWithFriendRelationAndTwoMeetingInvitation();
      var handler = new InviteToMeetingResponseCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContextWithFriendRelationAndMeetingInvitation()
    {
      var context = InitializedDbContext();

      var requests = new[]
      {
        new Relation(2, 1, RelationType.Friend),
        new Relation(3, 1, RelationType.Friend),
      };

      context.Relations.AddRange(requests);
      context.SaveChanges();

      var invitation = new MeetingInvitation(2, 1, 1);

      context.MeetingInvitations.AddRange(invitation);
      context.SaveChanges();

      return context;
    }

    private static SkelvyContext TestDbContextWithFriendRelationAndTwoMeetingInvitation()
    {
      var context = TestDbContextWithFriendRelationAndMeetingInvitation();

      var meeting = context.Meetings.FirstOrDefault(x => x.Id == 1);

      if (meeting != null)
      {
        meeting.Abort();
        context.Meetings.Update(meeting);
        context.SaveChanges();
      }

      return context;
    }
  }
}
