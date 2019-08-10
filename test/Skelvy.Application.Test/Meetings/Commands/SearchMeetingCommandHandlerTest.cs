using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.SearchMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class SearchMeetingCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<SearchMeetingCommandHandler>> _logger;

    public SearchMeetingCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
      _logger = new Mock<ILogger<SearchMeetingCommandHandler>>();
    }

    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = Request();
      request.MinDate = DateTimeOffset.UtcNow.AddDays(2);
      request.MaxDate = DateTimeOffset.UtcNow.AddDays(4);
      var dbContext = TestDbContextWithMeetings();
      var handler = new SearchMeetingCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
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
    public async Task ShouldAddRequest()
    {
      var request = Request();
      var dbContext = TestDbContext();
      var handler = new SearchMeetingCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
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
    public async Task ShouldCreateMeeting()
    {
      var request = Request();
      request.UserId = 2;
      var dbContext = TestDbContextWithRequests();
      var handler = new SearchMeetingCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
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
      var request = Request();
      request.UserId = 100;
      var dbContext = TestDbContext();
      var handler = new SearchMeetingCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
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
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.Activities[0].Id = 10;
      var dbContext = TestDbContext();
      var handler = new SearchMeetingCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
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
    public async Task ShouldThrowExceptionWithExistingRequest()
    {
      var request = Request();
      var dbContext = InitializedDbContext();
      var handler = new SearchMeetingCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new GroupsRepository(dbContext),
        new GroupUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    private static SearchMeetingCommand Request()
    {
      return new SearchMeetingCommand(
        1,
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow.AddDays(2),
        18,
        25,
        1,
        1,
        new List<CreateMeetingRequestActivity>
        {
          new CreateMeetingRequestActivity(1),
        });
    }

    private static SkelvyContext TestDbContext()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedActivities(context);
      return context;
    }

    private static SkelvyContext TestDbContextWithRequests()
    {
      var context = TestDbContext();
      SkelvyInitializer.SeedMeetingRequests(context);
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
