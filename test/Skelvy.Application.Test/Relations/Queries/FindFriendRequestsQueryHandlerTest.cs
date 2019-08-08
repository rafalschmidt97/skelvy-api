using System.Threading.Tasks;
using Skelvy.Application.Relations.Queries;
using Skelvy.Application.Relations.Queries.FindFriendRequests;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Queries
{
  public class FindFriendRequestsQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnFriends()
    {
      var request = new FindFriendRequestsQuery(2);
      var dbContext = InitializedDbContext();

      var handler = new FindFriendRequestsQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<FriendRequestDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindFriendRequestsQuery(1);
      var dbContext = InitializedDbContext();

      var handler = new FindFriendRequestsQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<FriendRequestDto>(x));
      Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindFriendRequestsQuery(1);
      var dbContext = DbContext();

      var handler = new FindFriendRequestsQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
