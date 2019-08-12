using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class JoinMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<JoinMeetingCommandHandler>> _logger;

    public JoinMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
      _logger = new Mock<ILogger<JoinMeetingCommandHandler>>();
    }

    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = new JoinMeetingCommand(1, 1);
      var dbContext = TestDbContextWithMeetings();
      var handler = new JoinMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new JoinMeetingCommand(100, 1);
      var dbContext = TestDbContext();
      var handler = new JoinMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidMeeting()
    {
      var request = new JoinMeetingCommand(1, 100);
      var dbContext = TestDbContext();
      var handler = new JoinMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContext()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedActivities(context);
      return context;
    }

    private static SkelvyContext TestDbContextWithMeetings()
    {
      var context = TestDbContext();
      SkelvyInitializer.SeedMeetings(context);
      return context;
    }
  }
}
