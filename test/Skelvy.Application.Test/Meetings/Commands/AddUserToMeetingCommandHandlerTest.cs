using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.AddUserToMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddUserToMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public AddUserToMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = new AddUserToMeetingCommand(2, 1, 1);
      var dbContext = TestDbContextWithRelations();
      var handler = new AddUserToMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new AddUserToMeetingCommand(100, 1, 1);
      var dbContext = TestDbContextWithRelations();
      var handler = new AddUserToMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidAddedUser()
    {
      var request = new AddUserToMeetingCommand(2, 1, 100);
      var dbContext = TestDbContextWithRelations();
      var handler = new AddUserToMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNonFriendRelation()
    {
      var request = new AddUserToMeetingCommand(2, 1, 4);
      var dbContext = InitializedDbContext();
      var handler = new AddUserToMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new AddUserToMeetingCommand(2, 100, 1);
      var dbContext = TestDbContextWithRelations();
      var handler = new AddUserToMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithAlreadyAddedUser()
    {
      var request = new AddUserToMeetingCommand(2, 1, 3);
      var dbContext = TestDbContextWithRelations();
      var handler = new AddUserToMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        new RelationsRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContextWithRelations()
    {
      var context = InitializedDbContext();

      var relations = new[]
      {
        new Relation(2, 1, RelationType.Friend),
        new Relation(1, 2, RelationType.Friend),
      };

      context.Relations.AddRange(relations);
      context.SaveChanges();

      return context;
    }
  }
}
