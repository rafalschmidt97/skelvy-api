using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindBlockedUsers;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindBlockedUsersQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnMessages()
    {
      var request = new FindBlockedUsersQuery(2, 1);
      var dbContext = InitializedDbContext();
      var handler = new FindBlockedUsersQueryHandler(
        new UsersRepository(dbContext),
        new BlockedUsersRepository(dbContext),
        Mapper());

      var result = await handler.Handle(request);

      Assert.All(result, x => Assert.IsType<UserDto>(x));
      Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindBlockedUsersQuery(1, 1);
      var dbContext = DbContext();
      var handler = new FindBlockedUsersQueryHandler(
        new UsersRepository(dbContext),
        new BlockedUsersRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
