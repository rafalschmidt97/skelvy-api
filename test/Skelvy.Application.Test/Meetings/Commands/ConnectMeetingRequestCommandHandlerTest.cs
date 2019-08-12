using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class ConnectMeetingRequestCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<ConnectMeetingRequestCommandHandler>> _logger;

    public ConnectMeetingRequestCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
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
        new MeetingRequestActivityRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
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
        new MeetingRequestActivityRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
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
        new MeetingRequestActivityRepository(dbContext),
        new GroupsRepository(dbContext),
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

    private static SkelvyContext TestDbContextWithMeetingRequests()
    {
      var context = TestDbContext();
      SkelvyInitializer.SeedMeetingRequests(context);
      return context;
    }
  }
}
