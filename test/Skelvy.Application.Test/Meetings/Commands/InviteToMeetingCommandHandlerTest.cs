using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.InviteToMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class InviteToMeetingCommandHandlerTest : DatabaseRequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public InviteToMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new InviteToMeetingCommand(2, 1, 1);
      var dbContext = TestDbContextWithFriendRelation();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingUser()
    {
      var request = new InviteToMeetingCommand(100, 1, 1);
      var dbContext = TestDbContextWithFriendRelation();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingInvitedUser()
    {
      var request = new InviteToMeetingCommand(2, 100, 1);
      var dbContext = TestDbContextWithFriendRelation();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingMeeting()
    {
      var request = new InviteToMeetingCommand(2, 1, 100);
      var dbContext = TestDbContextWithFriendRelation();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingGroupUser()
    {
      var request = new InviteToMeetingCommand(2, 3, 1);
      var dbContext = TestDbContextWithFriendRelation();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingInvitation()
    {
      var request = new InviteToMeetingCommand(2, 1, 1);
      var dbContext = TestDbContextWithFriendRelationAndMeetingInvitation();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonExistingFriendRelation()
    {
      var request = new InviteToMeetingCommand(2, 1, 1);
      var dbContext = InitializedDbContext();
      var handler = new InviteToMeetingCommandHandler(
        new MeetingInvitationsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new UsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContextWithFriendRelation()
    {
      var context = InitializedDbContext();

      var requests = new[]
      {
        new Relation(2, 1, RelationType.Friend),
        new Relation(3, 1, RelationType.Friend),
      };

      context.Relations.AddRange(requests);
      context.SaveChanges();

      return context;
    }

    private static SkelvyContext TestDbContextWithFriendRelationAndMeetingInvitation()
    {
      var context = TestDbContextWithFriendRelation();

      var invitation = new MeetingInvitation(2, 1, 1);

      context.MeetingInvitations.AddRange(invitation);
      context.SaveChanges();

      return context;
    }
  }
}
