using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Meetings.Queries
{
  public class FindMeetingSuggestionsQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnSuggestions()
    {
      var request = new FindMeetingSuggestionsQuery(4, 1, 1);
      var dbContext = InitializedDbContext();
      var handler = new FindMeetingSuggestionsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<MeetingSuggestionsModel>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithMatchedUser()
    {
      var request = new FindMeetingSuggestionsQuery(2, 1, 1);
      var dbContext = InitializedDbContext();
      var handler = new FindMeetingSuggestionsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<ConflictException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotExistingUser()
    {
      var request = new FindMeetingSuggestionsQuery(1, 1, 1);
      var dbContext = DbContext();
      var handler = new FindMeetingSuggestionsQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
