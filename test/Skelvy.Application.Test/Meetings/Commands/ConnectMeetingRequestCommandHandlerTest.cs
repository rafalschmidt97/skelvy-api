using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Common.Exceptions;
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
      var request = new ConnectMeetingRequestCommand(2, 1, DateTimeOffset.UtcNow.AddDays(1), 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = new ConnectMeetingRequestCommand(100, 1, DateTimeOffset.UtcNow.AddDays(1), 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
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
      var request = new ConnectMeetingRequestCommand(2, 100, DateTimeOffset.UtcNow.AddDays(1), 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithSelfRequest()
    {
      var request = new ConnectMeetingRequestCommand(1, 1, DateTimeOffset.UtcNow.AddDays(1), 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidActivityId()
    {
      var request = new ConnectMeetingRequestCommand(2, 1, DateTimeOffset.UtcNow.AddDays(1), 100);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<BadRequestException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDate()
    {
      var request = new ConnectMeetingRequestCommand(2, 1, DateTimeOffset.UtcNow.AddDays(10), 1);
      var dbContext = InitializedDbContext();
      var handler = new ConnectMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<BadRequestException>(() =>
        handler.Handle(request));
    }
  }
}
