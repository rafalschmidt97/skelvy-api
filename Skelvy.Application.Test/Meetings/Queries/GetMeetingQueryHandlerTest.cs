using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.GetMeeting;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class GetMeetingQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMeeting()
    {
      var request = new GetMeetingQuery { UserId = 2 };
      var handler = new GetMeetingQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<MeetingViewModel>(result);
      Assert.NotNull(result.Meeting);
    }

    [Fact]
    public async Task ShouldReturnRequest()
    {
      var request = new GetMeetingQuery { UserId = 1 };
      var handler = new GetMeetingQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<MeetingViewModel>(result);
      Assert.NotNull(result.Request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new GetMeetingQuery { UserId = 1 };
      var handler = new GetMeetingQueryHandler(DbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
