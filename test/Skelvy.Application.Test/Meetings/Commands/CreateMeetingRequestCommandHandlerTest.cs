using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Core.Initializers;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
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
      var handler = new CreateMeetingRequestCommandHandler(TestDbContextWithMeetings(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = Request();
      var handler = new CreateMeetingRequestCommandHandler(TestDbContext(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCreateMeeting()
    {
      var request = Request();
      request.UserId = 2;
      var handler = new CreateMeetingRequestCommandHandler(TestDbContextWithRequests(), _notifications.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = Request();
      request.UserId = 100;
      var handler = new CreateMeetingRequestCommandHandler(TestDbContext(), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.Drinks.First().Id = 10;
      var handler = new CreateMeetingRequestCommandHandler(TestDbContext(), _notifications.Object);

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRequest()
    {
      var request = Request();
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext(), _notifications.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = Request();
      request.UserId = 2;
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext(), _notifications.Object);

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static CreateMeetingRequestCommand Request()
    {
      return new CreateMeetingRequestCommand
      {
        UserId = 1,
        MinDate = DateTimeOffset.UtcNow,
        MaxDate = DateTimeOffset.UtcNow.AddDays(2),
        MinAge = 18,
        MaxAge = 25,
        Latitude = 1,
        Longitude = 1,
        Drinks = new List<CreateMeetingRequestDrink>
        {
          new CreateMeetingRequestDrink { Id = 1 }
        }
      };
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
