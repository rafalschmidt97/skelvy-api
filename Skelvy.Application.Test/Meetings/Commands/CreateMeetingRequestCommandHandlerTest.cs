using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Skelvy.Persistence;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class CreateMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldAddToExistingMeeting()
    {
      var request = Request();
      request.MinDate = DateTimeOffset.Now.AddDays(2);
      request.MaxDate = DateTimeOffset.Now.AddDays(4);
      var handler = new CreateMeetingRequestCommandHandler(TestDbContextWithMeetings());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = Request();
      var handler = new CreateMeetingRequestCommandHandler(TestDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldCreateMeeting()
    {
      var request = Request();
      request.UserId = 2;
      var handler = new CreateMeetingRequestCommandHandler(TestDbContextWithRequests());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = Request();
      request.UserId = 100;
      var handler = new CreateMeetingRequestCommandHandler(TestDbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.Drinks.First().Id = 10;
      var handler = new CreateMeetingRequestCommandHandler(TestDbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingRequest()
    {
      var request = Request();
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithExistingMeeting()
    {
      var request = Request();
      request.UserId = 2;
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static CreateMeetingRequestCommand Request()
    {
      return new CreateMeetingRequestCommand
      {
        UserId = 1,
        MinDate = DateTimeOffset.Now,
        MaxDate = DateTimeOffset.Now.AddDays(2),
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
