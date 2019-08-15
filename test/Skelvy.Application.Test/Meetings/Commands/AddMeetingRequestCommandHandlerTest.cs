using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.AddMeetingRequest;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class AddMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldAddRequest()
    {
      var request = Request();
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingRequestDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = Request();
      request.UserId = 100;
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        Mapper());
      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.Activities[0].Id = 100;
      var dbContext = InitializedDbContext();
      var handler = new AddMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        Mapper());
      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithTooManyRequests()
    {
      var request = Request();
      var dbContext = TestDbContextWithThreeRequests();
      var handler = new AddMeetingRequestCommandHandler(
        new UsersRepository(dbContext),
        new ActivitiesRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        Mapper());
      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    private static AddMeetingRequestCommand Request()
    {
      return new AddMeetingRequestCommand(
        1,
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow.AddDays(2),
        18,
        25,
        1,
        1,
        new List<AddMeetingRequestActivity>
        {
          new AddMeetingRequestActivity(1),
        });
    }

    private static SkelvyContext TestDbContextWithThreeRequests()
    {
      var context = DbContext();
      SkelvyInitializer.SeedUsers(context);
      SkelvyInitializer.SeedProfiles(context);
      SkelvyInitializer.SeedActivities(context);

      var requests = new[]
      {
        new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), 18, 25, 1, 1, 1),
        new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(2), 18, 25, 1, 1, 1),
        new MeetingRequest(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(3), 18, 25, 1, 1, 1),
      };

      context.MeetingRequests.AddRange(requests);
      context.SaveChanges();

      return context;
    }
  }
}
