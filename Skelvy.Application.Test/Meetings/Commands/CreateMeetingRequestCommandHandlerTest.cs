using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Commands
{
  public class CreateMeetingRequestCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = Request();
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidUser()
    {
      var request = Request();
      request.UserId = 2;
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithInvalidDrink()
    {
      var request = Request();
      request.Drinks.First().Id = 10;
      var handler = new CreateMeetingRequestCommandHandler(InitializedDbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static CreateMeetingRequestCommand Request()
    {
      return new CreateMeetingRequestCommand
      {
        UserId = 1,
        MinDate = DateTime.Now,
        MaxDate = DateTime.Now.AddDays(2),
        MinAge = 18,
        MaxAge = 25,
        Latitude = 0,
        Longitude = 0,
        Drinks = new List<CreateMeetingRequestDrink>
        {
          new CreateMeetingRequestDrink { Id = 1 }
        }
      };
    }
  }
}
