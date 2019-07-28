using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class CreateMeetingRequestCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<CreateMeetingRequestCommandHandler>> _logger;

    public CreateMeetingRequestCommandHandlerTest()
    {
      _mediator = new Mock<IMediator>();
      _logger = new Mock<ILogger<CreateMeetingRequestCommandHandler>>();
    }

    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = Request();
      request.MinDate = DateTimeOffset.UtcNow.AddDays(2);
      request.MaxDate = DateTimeOffset.UtcNow.AddDays(4);
      var dbContext = TestDbContextWithMeetings();
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinkTypesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = Request();
      var dbContext = TestDbContext();
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinkTypesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
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
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinkTypesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
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
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinkTypesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.DrinkTypes[0].Id = 10;
      var dbContext = TestDbContext();
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinkTypesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
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
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinkTypesRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestDrinkTypesRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    private static CreateMeetingRequestCommand Request()
    {
      return new CreateMeetingRequestCommand(
        1,
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow.AddDays(2),
        18,
        25,
        1,
        1,
        new List<CreateMeetingRequestDrinkType>
        {
          new CreateMeetingRequestDrinkType(1),
        });
    }

    private static SkelvyContext TestDbContext()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedDrinkTypes(context);
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
