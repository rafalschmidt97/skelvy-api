using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMeeting()
    {
      var request = new FindMeetingQuery(2);
      var handler = new FindMeetingQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingViewModel>(result);
      Assert.NotNull(result.Meeting);
    }

    [Fact]
    public async Task ShouldReturnRequest()
    {
      var request = new FindMeetingQuery(1);
      var handler = new FindMeetingQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingViewModel>(result);
      Assert.NotNull(result.Request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingQuery(1);
      var handler = new FindMeetingQueryHandler(DbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
