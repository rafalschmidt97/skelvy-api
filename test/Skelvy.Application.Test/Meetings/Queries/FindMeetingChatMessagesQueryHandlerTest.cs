using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
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
      var request = new FindMeetingChatMessagesQuery(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new FindMeetingChatMessagesQueryHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<MeetingChatMessageDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindMeetingChatMessagesQuery(1, 1);
      var dbContext = DbContext();
      var handler = new FindMeetingChatMessagesQueryHandler(
        new MeetingUsersRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
