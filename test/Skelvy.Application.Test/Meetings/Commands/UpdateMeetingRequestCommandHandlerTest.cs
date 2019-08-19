using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Commands.UpdateMeetingRequest;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class UpdateMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldUpdateRequest()
    {
      var request = Request();
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidRequest()
    {
      var request = Request();
      request.RequestId = 100;
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = Request();
      request.UserId = 100;
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await Assert.ThrowsAsync<ForbiddenException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidActivity()
    {
      var request = Request();
      request.Activities[0].Id = 100;
      var dbContext = InitializedDbContext();
      var handler = new UpdateMeetingRequestCommandHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingRequestActivityRepository(dbContext),
        new ActivitiesRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static UpdateMeetingRequestCommand Request()
    {
      return new UpdateMeetingRequestCommand(
        1,
        1,
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow.AddDays(2),
        18,
        25,
        1,
        1,
        null,
        new List<UpdateMeetingRequestActivity>
        {
          new UpdateMeetingRequestActivity(1),
        });
    }
  }
}
