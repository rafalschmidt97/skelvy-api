using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class JoinMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;
    private readonly Mock<ILogger<JoinMeetingCommandHandler>> _logger;

    public JoinMeetingCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
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
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object,
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
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object,
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
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object,
        _logger.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRequest()
    {
      var request = new JoinMeetingCommand(1, 1);
      var dbContext = InitializedDbContext();
      var handler = new JoinMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object,
        _logger.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = new JoinMeetingCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new JoinMeetingCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object,
        _logger.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext TestDbContext()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedDrinkTypes(context);
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