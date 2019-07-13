using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class ConnectMeetingRequestCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;
    private readonly Mock<ILogger<ConnectMeetingRequestCommandHandler>> _logger;

    public ConnectMeetingRequestCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
      _logger = new Mock<ILogger<ConnectMeetingRequestCommandHandler>>();
    }

    [Fact]
    public async Task ShouldConnectRequests()
    {
      var request = new ConnectMeetingRequestCommand(2, 1);
      var dbContext = TestDbContextWithMeetingRequests();
      var handler = new ConnectMeetingRequestCommandHandler(
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
      var request = new ConnectMeetingRequestCommand(100, 1);
      var dbContext = TestDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
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
    public async Task ShouldThrowExceptionWithInvalidMeetingRequest()
    {
      var request = new ConnectMeetingRequestCommand(1, 100);
      var dbContext = TestDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
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
    public async Task ShouldConnectWithExistingRequest()
    {
      var request = new ConnectMeetingRequestCommand(1, 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
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
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = new ConnectMeetingRequestCommand(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
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

    private static SkelvyContext TestDbContextWithMeetingRequests()
    {
      var context = TestDbContext();
      SkelvyInitializer.SeedMeetingRequests(context);
      return context;
    }
  }
}
