using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMeeting()
    {
      var request = new FindMeetingQuery(2);
      var dbContext = InitializedDbContext();
      var handler = new FindMeetingQueryHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingModel>(result);
      Assert.NotNull(result.Meeting);
    }

    [Fact]
    public async Task ShouldReturnRequest()
    {
      var request = new FindMeetingQuery(1);
      var dbContext = InitializedDbContext();
      var handler = new FindMeetingQueryHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingModel>(result);
      Assert.NotNull(result.Request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingQuery(1);
      var dbContext = DbContext();
      var handler = new FindMeetingQueryHandler(
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
