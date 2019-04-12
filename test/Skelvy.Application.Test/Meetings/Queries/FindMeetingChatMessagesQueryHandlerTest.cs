using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingChatMessages;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingChatMessagesQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMessages()
    {
      var request = new FindMeetingChatMessagesQuery
      {
        UserId = 2,
        Page = 1,
      };
      var handler = new FindMeetingChatMessagesQueryHandler(InitializedDbContext(), Mapper());

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.All(result, x => Assert.IsType<MeetingChatMessageDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingChatMessagesQuery
      {
        UserId = 1,
        Page = 1,
      };
      var handler = new FindMeetingChatMessagesQueryHandler(DbContext(), Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
