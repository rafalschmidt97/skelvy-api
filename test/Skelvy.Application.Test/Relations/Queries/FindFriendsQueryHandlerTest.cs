using System.Threading.Tasks;
using Skelvy.Application.Relations.Queries.FindFriends;
using Skelvy.Application.Users.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Queries
{
  public class FindFriendsQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnFriends()
    {
      var request = new FindFriendsQuery(2, 1);
      var dbContext = InitializedDbContext();

      var handler = new FindFriendsQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<UserDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldReturnEmpty()
    {
      var request = new FindFriendsQuery(1, 1);
      var dbContext = InitializedDbContext();

      var handler = new FindFriendsQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<UserDto>(x));
      Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindFriendsQuery(1, 1);
      var dbContext = DbContext();

      var handler = new FindFriendsQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
