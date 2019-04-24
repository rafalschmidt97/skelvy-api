using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class CreateMeetingRequestCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<INotificationsService> _notifications;

    public CreateMeetingRequestCommandHandlerTest()
    {
      _notifications = new Mock<INotificationsService>();
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
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = Request();
      var dbContext = TestDbContext();
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

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
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

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
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.Drinks[0].Id = 10;
      var dbContext = TestDbContext();
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

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
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = Request();
      request.UserId = 2;
      var dbContext = InitializedDbContext();
      var handler = new CreateMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new DrinksRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingUsersRepository(dbContext),
        _notifications.Object);

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
        new List<CreateMeetingRequestDrink>
        {
          new CreateMeetingRequestDrink(1),
        });
    }

    private static SkelvyContext TestDbContext()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedDrinks(context);
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
