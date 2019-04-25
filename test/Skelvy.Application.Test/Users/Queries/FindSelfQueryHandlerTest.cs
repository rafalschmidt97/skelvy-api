using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelf;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindSelfQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnModel()
    {
      var request = new FindSelfQuery(1);
      var dbContext = InitializedDbContext();
      var handler = new FindSelfQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.IsType<SelfModel>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindSelfQuery(1);
      var dbContext = DbContext();
      var handler = new FindSelfQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MeetingChatMessagesRepository(dbContext),
        Mapper());
      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
