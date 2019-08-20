using System.Threading.Tasks;
using Skelvy.Application.Relations.Queries.FindBlocked;
using Skelvy.Application.Users.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Relations.Queries
{
  public class FindBlockedQueryHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldReturnBlocked()
    {
      var request = new FindBlockedQuery(2, 1);
      var dbContext = InitializedDbContext();

      var handler = new FindBlockedQueryHandler(
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
      var request = new FindBlockedQuery(1, 1);
      var dbContext = InitializedDbContext();

      var handler = new FindBlockedQueryHandler(
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
      var request = new FindBlockedQuery(1, 1);
      var dbContext = DbContext();

      var handler = new FindBlockedQueryHandler(
        new RelationsRepository(dbContext),
        new UsersRepository(dbContext),
        Mapper());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}
