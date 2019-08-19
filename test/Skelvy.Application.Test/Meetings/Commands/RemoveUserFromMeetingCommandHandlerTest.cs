using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class RemoveUserFromMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;

    public RemoveUserFromMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = new RemoveUserFromMeetingCommand(2, 1, 3);
      var dbContext = TestDbContextWithPrivateMeeting();
      var handler = new RemoveUserFromMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new RemoveUserFromMeetingCommand(2, 100, 3);
      var dbContext = TestDbContextWithPrivateMeeting();
      var handler = new RemoveUserFromMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new RemoveUserFromMeetingCommand(100, 1, 3);
      var dbContext = TestDbContextWithPrivateMeeting();
      var handler = new RemoveUserFromMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidAddedUser()
    {
      var request = new RemoveUserFromMeetingCommand(2, 1, 100);
      var dbContext = TestDbContextWithPrivateMeeting();
      var handler = new RemoveUserFromMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithoutPermission()
    {
      var request = new RemoveUserFromMeetingCommand(3, 1, 2);
      var dbContext = TestDbContextWithPrivateMeeting();
      var handler = new RemoveUserFromMeetingCommandHandler(
        new MeetingsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object);

      await Assert.ThrowsAsync<ForbiddenException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContextWithPrivateMeeting()
    {
      var context = InitializedDbContext();

      var meeting = context.Meetings.FirstOrDefault(x => x.Id == 1);

      if (meeting != null)
      {
        meeting.IsPrivate = true;
        context.Meetings.Update(meeting);
        context.SaveChanges();
      }

      return context;
    }
  }
}
